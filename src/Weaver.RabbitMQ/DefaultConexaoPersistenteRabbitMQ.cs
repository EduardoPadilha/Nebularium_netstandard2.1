using Microsoft.Extensions.Logging;
using Nebularium.Weaver.RabbitMQ.Interfaces;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Net.Sockets;

namespace Nebularium.Weaver.RabbitMQ
{
    public class DefaultConexaoPersistenteRabbitMQ : IConexaoPersistenteRabbitMQ, IDisposable
    {

        private readonly IConnectionFactory _fabrica;
        private readonly ILogger<DefaultConexaoPersistenteRabbitMQ> _logger;
        private readonly int _tentativasReconexao;
        private IConnection _conexao;
        bool _disposed;

        object _sync_root = new object();

        public DefaultConexaoPersistenteRabbitMQ(IConnectionFactory factory, ILogger<DefaultConexaoPersistenteRabbitMQ> logger, int tentativasReconexao = 5)
        {
            _fabrica = factory;
            _logger = logger;
            _tentativasReconexao = tentativasReconexao;
        }
        public bool EstaConectado => _conexao != null && _conexao.IsOpen && !_disposed;

        public bool TentaConectar()
        {
            _logger.LogInformation("Cliente RabbitMQ está tentando conectar");

            lock (_sync_root)
            {
                if (EstaConectado) return true;

                var policy = Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_tentativasReconexao, tentarNovamente => TimeSpan.FromSeconds(Math.Pow(2, tentarNovamente)),
                    (ex, tempo) => { _logger.LogWarning(ex.ToString()); }
                );

                policy.Execute(() =>
                {
                    _conexao = _fabrica.CreateConnection();
                });

                if (!EstaConectado)
                {
                    _logger.LogCritical("ERRO FATAL: Conexões RabbitMQ não puderam ser criadas nem abertas");
                    return false;
                }

                ObservarSaudeConexao();

                _logger.LogInformation($"Nova conexão para {_conexao.Endpoint.HostName}.");

                return true;
            }
        }
        private void ObservarSaudeConexao()
        {
            _conexao.ConnectionShutdown += (sender, e) =>
            {
                if (_disposed) return;
                _logger.LogWarning("Uma conexão RabbitMQ foi fechada. Tentando reconectar...");
                TentaConectar();
            };

            _conexao.CallbackException += (sender, e) =>
            {
                if (_disposed) return;
                _logger.LogWarning("Uma conexão RabbitMQ soltou uma exceção. Tentando reconectar...");
                TentaConectar();
            };

            _conexao.ConnectionBlocked += (sender, e) =>
            {
                if (_disposed) return;
                _logger.LogWarning("Uma conexão RabbitMQ foi bloqueada. Tentando reconectar...");
                TentaConectar();
            };
        }
        public IModel CriaModelo()
        {
            if (!EstaConectado)
                throw new InvalidOperationException("Existem conexões RabbitMQ disponíveis para realizar esta ação");

            return _conexao.CreateModel();
        }
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try
            {
                _conexao.Dispose();
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }
    }
}
