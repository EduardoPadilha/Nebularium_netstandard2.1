using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Net.Sockets;

namespace Nebularium.Weaver.RabbitMQ
{
    public delegate Policy FabricaPoliticasConexoesPersistentes(ILogger<ConexaoPersistente> logger);
    public class ConexaoPersistente : IDisposable
    {
        public static readonly FabricaPoliticasConexoesPersistentes FabricaPolitacasPadrao = (logger) =>
        {
            return Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(5, tentarNnovamente => TimeSpan.FromSeconds(Math.Pow(2, tentarNnovamente)),
                    (ex, tempo) => { logger.LogWarning(ex.ToString()); }
                );
        };

        private readonly IConnectionFactory _fabrica;
        private readonly ILogger<ConexaoPersistente> _logger;
        private readonly FabricaPoliticasConexoesPersistentes _fabricaPoliticas;
        private readonly object _lockObject = new object();

        private IConnection _conexao;
        public bool EstaConectado => _conexao != null && _conexao.IsOpen && !Disposed;

        public ConexaoPersistente(
            IConnectionFactory factory,
            ILogger<ConexaoPersistente> logger,
            FabricaPoliticasConexoesPersistentes policyFactory
            )
        {
            _fabrica = factory;
            _logger = logger;
            _fabricaPoliticas = policyFactory ?? FabricaPolitacasPadrao;
        }

        public bool TentaConectar()
        {
            _logger.LogInformation("Cliente RabbitMQ está tentando conectar");

            lock (_lockObject)
            {
                if (EstaConectado) return true;

                var policy = _fabricaPoliticas(_logger);
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
                if (Disposed) return;
                _logger.LogWarning("Uma conexão RabbitMQ foi fechada. Tentando reconectar...");
                TentaConectar();
            };

            _conexao.CallbackException += (sender, e) =>
            {
                if (Disposed) return;
                _logger.LogWarning("Uma conexão RabbitMQ soltou uma exceção. Tentando reconectar...");
                TentaConectar();
            };

            _conexao.ConnectionBlocked += (sender, e) =>
            {
                if (Disposed) return;
                _logger.LogWarning("Uma conexão RabbitMQ foi bloqueada. Tentando reconectar...");
                TentaConectar();
            };
        }
        public IModel CreateModel()
        {
            if (!EstaConectado)
                throw new InvalidOperationException("Existem conexões RabbitMQ disponíveis para realizar esta ação");

            return _conexao.CreateModel();
        }
        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;

            try
            {
                _conexao.Dispose();
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }
        public bool Disposed { get; private set; }
    }
}
