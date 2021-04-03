using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Behemoth.Mongo.Configuracoes;
using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tarrasque.Configuracoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tarrasque.Recursos;
using Nebularium.Tellurian.Drone.Behemoth.Repositorios;
using Nebularium.Tellurian.Drone.Comandos;
using Nebularium.Tellurian.Drone.Eventos;
using Nebularium.Tellurian.Drone.Features;
using Nebularium.Tellurian.Drone.Interfaces.Comandos;
using Nebularium.Tellurian.Drone.Interfaces.Repositorios;
using Nebularium.Tellurian.Drone.Manipuladores;
using Nebularium.Tellurian.Recursos;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Weaver;
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
        public static IServiceCollection AddFeatures(this IServiceCollection servicos)
        {
            servicos.AddScoped<ICadastrarPessoa, CadastrarPessoa>();

            return servicos;
        }

        public static IServiceCollection AddComandos(this IServiceCollection servicos)
        {
            servicos.AddScoped<IPessoaComandos, PessoaComandos>();

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

            servicos.AddSingletonTodosPorInterface(typeof(IValidador<>), typeof(ServicoExtensao));
            servicos.AddSingleton<IGestorConfiguracao, GestorConfiguracaoPadrao>();
            servicos.AddSingleton<IDisplayNameExtrator>(sp => new DisplayNameExtratorPadrao());

            return servicos;
        }

        public static IServiceCollection AddRepositorios(this IServiceCollection servicos)
        {
            servicos.AddSingleton<IDbConfiguracao, DBConfiguracaoPadrao>();
            servicos.AddSingleton<IMongoContexto, TellurianContext>();

            servicos.AddScopedTodosPorInterface(typeof(IComandoRepositorioBase<>), typeof(ServicosExtensao));
            servicos.AddScopedTodosPorInterface(typeof(IComandoRepositorio<>), typeof(ServicosExtensao));
            servicos.AddScopedTodosPorInterface(typeof(IConsultaRepositorio<>), typeof(ServicosExtensao));
            servicos.AddScopedTodosPorInterface(typeof(IConsultaRepositorioBase<>), typeof(ServicosExtensao));

            servicos.AddScoped<IPessoaConsultaRepositorio, PessoaConsultaRepositorio>();
            servicos.AddScoped<IPessoaComandoRepositorio, PessoaComandoRepositorio>();

            return servicos;
        }

        public static IServiceCollection AddRepositoriosAutoMapper(this IServiceCollection servicos)
        {
            GestorAutoMapper.Inicializar();
            servicos.AddSingleton(sp => GestorAutoMapper.Instancia.Mapper);

            return servicos;
        }

        public static IServiceCollection AddIntegrationServices(this IServiceCollection servicos, IConfiguration configuracao)
        {

            servicos.AddSingleton<IConexaoPersistente>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultConexaoPersistente>>();

                var factory = new ConnectionFactory()
                {
                    HostName = configuracao["EventBusConnection"],
                    DispatchConsumersAsync = true
                };


                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuracao["EventBusRetryCount"]))
                    retryCount = int.Parse(configuracao["EventBusRetryCount"]);

                return new DefaultConexaoPersistente(factory, logger, retryCount);
            });

            return servicos;
        }

        public static IServiceCollection AddBarramentoEventos(this IServiceCollection servicos, IConfiguration configuracao)
        {
            var subscriptionClientName = configuracao["SubscriptionClientName"];

            servicos.AddSingleton<IBarramentoEvento, BarramentoEvento>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IConexaoPersistente>();
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
