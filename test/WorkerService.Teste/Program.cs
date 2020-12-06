using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nebularium.Tellurian.Drone.Eventos;
using Nebularium.Tellurian.Drone.Recursos;
using Nebularium.Weaver.Interfaces;
using Nebularium.Tellurian.Drone.Manipuladores;

namespace WorkerService.Teste
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Run();
        }

        public static IHost CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", false, true);
                })
                .ConfigureServices((hostContext, servicos) =>
                {
                    //servicos.AddHostedService<Worker>();
                    servicos.AddRecursos()
                            .AddValidadores()
                            .AddFiltros()
                            .AddDbContexto(hostContext.Configuration)
                            .AddServicos()
                            .AddIntegrationServices(hostContext.Configuration)
                            .AddBarramentoEventos(hostContext.Configuration);

                    servicos.AddTransient<CadastrarPessoaManipulador>();

                }).Build();
            host.Services.ConfigurarBarramentoEvento();
            FabricaProvider.Inicializar();
            var fabrica = (FabricaProvider)FabricaProvider.Instancia;
            fabrica.AddContainer(host.Services);
            return host;
        }
    }
}
