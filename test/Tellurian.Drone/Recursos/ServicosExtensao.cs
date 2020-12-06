using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nebularium.Behemoth.Mongo.Contextos;
using Nebularium.Tarrasque.Configuracoes;
using Nebularium.Tarrasque.Interfaces;
using Nebularium.Tarrasque.Recursos;
using Nebularium.Tellurian.Drone.Behemoth.Repositorios;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tellurian.Drone.Eventos;
using Nebularium.Tellurian.Drone.Interfaces;
using Nebularium.Tellurian.Drone.Manipuladores;
using Nebularium.Tellurian.Drone.Servicos;
using Nebularium.Tellurian.Mock;
using Nebularium.Tellurian.Recursos;
using Nebularium.Tiamat.Interfaces;
using Nebularium.Weaver.Interfaces;
using Nebularium.Weaver.RabbitMQ;
using Nebularium.Weaver.RabbitMQ.Interfaces;
using RabbitMQ.Client;
using Serilog;
using System;

namespace Nebularium.Tellurian.Drone.Recursos
{
    public static class ServicosExtensao
    {
        public static IServiceCollection AddFiltros(this IServiceCollection servicos)
        {
            //servicos.AddSingleton<IValidador<Pessoa>, PessoaValidador>();
            //servicos.AddSingleton<IValidador<Endereco>, EnderecoValidador>();

            return servicos;
        }

        public static IServiceCollection AddServicos(this IServiceCollection servicos)
        {
            servicos.AddSingleton<IComandoServico<Pessoa>, PessoaComandoServico>();
            //servicos.AddSingleton<IValidador<Endereco>, EnderecoValidador>();

            return servicos;
        }

        public static IServiceCollection AddValidadores(this IServiceCollection servicos)
        {
            servicos.AddSingleton<IValidador<Pessoa>, PessoaValidador>();
            servicos.AddSingleton<IValidador<Endereco>, EnderecoValidador>();

            return servicos;
        }

        public static IServiceCollection AddRecursos(this IServiceCollection servicos)
        {
            servicos.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(new LoggerConfiguration()
                                .WriteTo.Console()
                                .WriteTo.Debug()
                                .WriteTo.File("log.txt")
                                .CreateLogger(), true);
            });

            GestorAutoMapper.Inicializar();
            servicos.AddSingleton(sp => GestorAutoMapper.Instancia.Mapper);

            servicos.AddSingleton<IGestorConfiguracao, GestorConfiguracaoPadrao>();

            servicos.AddSingleton<IDisplayNameExtrator>(sp => new DisplayNameExtratorPadrao());

            return servicos;
        }

        public static IServiceCollection AddDbContexto(this IServiceCollection servicos, IConfiguration configuracao)
        {
            servicos.AddSingleton<IDbConfigs>(sp => new DBConfig(configuracao));
            servicos.AddSingleton<IMongoContext, TellurianContext>();

            servicos.AddTransient<IPessoaConsultaRepositorio, PessoaConsultaRepositorio>();
            servicos.AddTransient<IComandoRepositorio<Pessoa>, PessoaComandoRepositorio>();

            return servicos;
        }

        public static IServiceCollection AddIntegrationServices(this IServiceCollection servicos, IConfiguration configuracao)
        {

            servicos.AddSingleton<IConexaoPersistenteRabbitMQ>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultConexaoPersistenteRabbitMQ>>();

                var factory = new ConnectionFactory()
                {
                    HostName = configuracao["EventBusConnection"],
                    DispatchConsumersAsync = true
                };


                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuracao["EventBusRetryCount"]))
                    retryCount = int.Parse(configuracao["EventBusRetryCount"]);

                return new DefaultConexaoPersistenteRabbitMQ(factory, logger, retryCount);
            });

            return servicos;
        }

        public static IServiceCollection AddBarramentoEventos(this IServiceCollection servicos, IConfiguration configuracao)
        {
            var subscriptionClientName = configuracao["SubscriptionClientName"];

            servicos.AddSingleton<IBarramentoEvento, BarramentoEvento>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IConexaoPersistenteRabbitMQ>();
                var logger = sp.GetRequiredService<ILogger<BarramentoEvento>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IGerenciadorAssinatura>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuracao["EventBusRetryCount"]))
                    retryCount = int.Parse(configuracao["EventBusRetryCount"]);

                return new BarramentoEvento(rabbitMQPersistentConnection, logger, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });

            servicos.AddSingleton<IGerenciadorAssinatura, GerenciadorAssinatura>();
            //Handlers
            //servicos.AddTransient<CadastrarPessoaManipulador>();
            //services.AddTransient<OrderStatusChangedToShippedIntegrationEventHandler>();
            //services.AddTransient<OrderStatusChangedToPaidIntegrationEventHandler>();
            return servicos;
        }

        public static void ConfigurarBarramentoEvento(this IServiceProvider container)
        {
            var barramentoEvento = container.GetRequiredService<IBarramentoEvento>();
            barramentoEvento.ConfigurarBarramentoEvento();
        }

        private static void ConfigurarBarramentoEvento(this IBarramentoEvento barramento)
        {
            barramento.Assinar<CadastrarPessoaComandoEvento, CadastrarPessoaManipulador>();
        }
    }
}
