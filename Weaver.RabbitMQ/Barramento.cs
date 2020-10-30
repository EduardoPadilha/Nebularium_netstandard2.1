using Microsoft.Extensions.Logging;
using Nebularium.Weaver.Interfaces;
using Nebularium.Weaver.RabbitMQ;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Nebularium.Weaver
{
    public class Barramento : IBarramento
    {
        private readonly ConexaoPersistente _conexao;
        private readonly ILogger _logger;
        private readonly GerenciadorAssinatura _gerenciadorAssinatura;

        private string _filaNome;
        private IModel _canalConsumidor;

        public string ExchangeName { get; }

        public Barramento(ConexaoPersistente connection, ILogger logger,
            string exchangeName = "DefaultExchange")
        {

            _conexao = connection
                ?? throw new ArgumentNullException(nameof(connection));

            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));

            ExchangeName = exchangeName;

            _gerenciadorAssinatura = new GerenciadorAssinatura();
            _gerenciadorAssinatura.QuandoEventoRemovido += QuandoGerenciadorEventoAssinaturaRemovido;
            _gerenciadorAssinatura.QuandoEventoAdicionado += QuandoGerenciadorEventoAssinaturaAdicionado;

            _canalConsumidor = CriaCanalConsumidor();
        }

        void QuandoGerenciadorEventoAssinaturaAdicionado(object _, Type tipo)
        {
            if (!_conexao.EstaConectado)
                _conexao.TentaConectar();

            using (var canal = _conexao.CreateModel())
                canal.QueueBind(_filaNome, ExchangeName, tipo.FullName);
        }

        void QuandoGerenciadorEventoAssinaturaRemovido(object _, Type tipo)
        {
            if (!_conexao.EstaConectado)
                _conexao.TentaConectar();

            using (var canal = _conexao.CreateModel())
            {
                canal.QueueUnbind(_filaNome, ExchangeName, tipo.FullName);

                if (!_gerenciadorAssinatura.EstaVazio) return;

                _filaNome = string.Empty;
                _canalConsumidor.Close();
            }
        }

        public void Publicar(IEvento evento)
        {
            if (!_conexao.EstaConectado)
                _conexao.TentaConectar();

            var policy = Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(5, tentarNnovamente => TimeSpan.FromSeconds(Math.Pow(2, tentarNnovamente)),
                    (ex, tempo) => { _logger.LogWarning(ex.ToString()); }
                );

            using (var canal = _conexao.CreateModel())
            {
                var eventoNome = evento.GetType().FullName;

                canal.ExchangeDeclare(exchange: ExchangeName,
                    type: "direct");

                var message = JsonConvert.SerializeObject(evento);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    canal.BasicPublish(ExchangeName, eventoNome, null, body);
                });
            }
        }

        public void Assinar<TEvento, TEventoManipulado>()
            where TEvento : IEvento
            where TEventoManipulado : IEventoManipulador<TEvento>
        {
            _gerenciadorAssinatura.AddAssinatura<TEvento, TEventoManipulado>();
        }
        public void CancelarAssinatura<TEvento, TEventoManipulado>()
            where TEvento : IEvento
            where TEventoManipulado : IEventoManipulador<TEvento>
        {
            _gerenciadorAssinatura.RemoverAssinatura<TEvento, TEventoManipulado>();
        }
        private IModel CriaCanalConsumidor()
        {
            if (!_conexao.EstaConectado)
                _conexao.TentaConectar();

            var canal = _conexao.CreateModel();
            canal.ExchangeDeclare(ExchangeName, "direct");
            _filaNome = canal.QueueDeclare().QueueName;

            var consumidor = new EventingBasicConsumer(canal);
            consumidor.Received += async (model, ea) =>
            {
                var eventoNome = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                await ManipularEvento(Type.GetType(eventoNome), message);
            };

            canal.BasicConsume(_filaNome, false, consumidor);

            canal.CallbackException += (sender, ea) =>
            {
                _canalConsumidor.Dispose();
                _canalConsumidor = CriaCanalConsumidor();
            };

            return canal;
        }

        private async Task ManipularEvento(Type tipoEvento, string mensagem)
        {
            if (!_gerenciadorAssinatura.TemAssinaturaParaEvento(tipoEvento))
                return;

            var assinaturas = _gerenciadorAssinatura.ObterManipuladores(tipoEvento);
            foreach (var assinatura in assinaturas)
                await assinatura.Resolver(mensagem);
        }

        public void Dispose()
        {
            _canalConsumidor?.Dispose();
        }
    }
}
