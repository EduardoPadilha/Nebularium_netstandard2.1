using Microsoft.Extensions.Logging;
using Nebularium.Weaver.Exceccoes;
using Nebularium.Weaver.Interfaces;
using Nebularium.Weaver.RabbitMQ.Interfaces;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Nebularium.Weaver.RabbitMQ
{
    public class BarramentoEvento : IBarramentoEvento
    {
        const string BROKER_NAME = "noctua_event_bus";
        private readonly IConexaoPersistente _conexao;
        private readonly ILogger _logger;
        private readonly IGerenciadorAssinatura _gerenciadorAssinatura;
        private readonly int _retryCount;
        private string _filaNome;
        private IModel _canalConsumidor;
        //private readonly string _exchangeName;
        public BarramentoEvento(IConexaoPersistente connection, ILogger logger,
            IGerenciadorAssinatura gerenciadorAssinatura, string filaNome = null, int retryCount = 5)
        {

            _conexao = connection
                ?? throw new ArgumentNullException(nameof(connection));

            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));

            //_exchangeName = exchangeName;
            _filaNome = filaNome;
            _retryCount = retryCount;

            _gerenciadorAssinatura = gerenciadorAssinatura ?? new GerenciadorAssinatura();
            _gerenciadorAssinatura.QuandoEventoRemovido += QuandoEventoRemovido;
            _gerenciadorAssinatura.QuandoEventoAdicionado += QuandoEventoAdicionado;

            _canalConsumidor = CriaCanalConsumidor();
        }

        public virtual void Publicar(IEvento evento)
        {
            if (!_conexao.EstaConectado)
                _conexao.TentaConectar();

            var policy = Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_retryCount, tentarNnovamente => TimeSpan.FromSeconds(Math.Pow(2, tentarNnovamente)),
                    (ex, tempo) => { _logger.LogWarning(ex.ToString()); }
                );

            var eventoNome = evento.GetType().Name;

            _logger.LogTrace($"Criando canal para publicar evento: [{eventoNome}] - {evento.Id}");
            using (var canal = _conexao.CriaModelo())
            {
                _logger.LogTrace($"Declarando RabbitMQ exchange para publicar evento: {evento.Id}");
                canal.ExchangeDeclare(BROKER_NAME, ExchangeType.Direct);

                var message = JsonConvert.SerializeObject(evento);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    var properties = canal.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent
                    _logger.LogTrace($"Publicando evento no RabbitMQ: {evento.Id}");
                    canal.BasicPublish(BROKER_NAME, eventoNome, true, properties, body);
                });
            }
        }
        public void Assinar<TEvento, TManipuladoEvento>()
            where TEvento : IEvento
            where TManipuladoEvento : IManipuladorEvento<TEvento>
        {
            _gerenciadorAssinatura.AddAssinatura<TEvento, TManipuladoEvento>();
            IniciarCanalConsumidor();
        }
        public virtual void CancelarAssinatura<TEvento, TManipuladoEvento>()
            where TEvento : IEvento
            where TManipuladoEvento : IManipuladorEvento<TEvento>
        {
            _gerenciadorAssinatura.RemoverAssinatura<TEvento, TManipuladoEvento>();
        }
        #region Suportes

        private void QuandoEventoAdicionado(object _, string tipo)
        {
            if (!_conexao.EstaConectado)
                _conexao.TentaConectar();

            using (var canal = _conexao.CriaModelo())
                canal.QueueBind(_filaNome, BROKER_NAME, tipo);
        }
        private void QuandoEventoRemovido(object _, string tipo)
        {
            if (!_conexao.EstaConectado)
                _conexao.TentaConectar();

            using (var canal = _conexao.CriaModelo())
            {
                canal.QueueUnbind(_filaNome, BROKER_NAME, tipo);

                if (!_gerenciadorAssinatura.EstaVazio) return;

                _filaNome = string.Empty;
                _canalConsumidor.Close();
            }
        }
        private IModel CriaCanalConsumidor()
        {
            if (!_conexao.EstaConectado)
                _conexao.TentaConectar();

            _logger.LogTrace("Criando canal consumidor RabbitMQ");

            var canal = _conexao.CriaModelo();
            canal.ExchangeDeclare(BROKER_NAME, ExchangeType.Direct);
            canal.QueueDeclare(_filaNome, durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            canal.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recriando canal consumidor");

                _canalConsumidor.Dispose();
                _canalConsumidor = CriaCanalConsumidor();

                IniciarCanalConsumidor();
            };

            return canal;
        }
        private void IniciarCanalConsumidor()
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");
            if (_canalConsumidor == null)
            {
                _logger.LogError("IniciarCanalConsumidor não pode chamar no _canalConsumidor == null");
                return;
            }
            var consumidor = new AsyncEventingBasicConsumer(_canalConsumidor);
            consumidor.Received += async (model, ea) =>
            {
                var eventoNome = ea.RoutingKey;
                var mensagem = Encoding.UTF8.GetString(ea.Body.ToArray());
                try
                {
                    await ProcessarEvento(eventoNome, mensagem);
                    _canalConsumidor.BasicAck(ea.DeliveryTag, false);
                }
                catch (SemManipuladorException ex)
                {
                    _logger.LogWarning(ex, $"----- ERRO processando mensagem \"{mensagem}\"");
                    _canalConsumidor.BasicNack(ea.DeliveryTag, true, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"----- ERRO processando mensagem \"{mensagem}\"");
                }
            };

            _canalConsumidor.BasicConsume(_filaNome, false, consumidor);
        }
        private async Task ProcessarEvento(string tipoEvento, string mensagem)
        {
            if (!_gerenciadorAssinatura.TemAssinaturaParaEvento(tipoEvento))
                return;

            var assinaturas = _gerenciadorAssinatura.ObterManipuladores(tipoEvento);
            foreach (var assinatura in assinaturas)
                await assinatura.Resolver(mensagem);
        }

        #endregion
        public void Dispose()
        {
            _canalConsumidor?.Dispose();
        }
    }
}
