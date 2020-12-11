using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tellurian.Drone.Manipuladores;
using System;

namespace Nebularium.Tellurian.Drone.Recursos
{
    public class NebulariumFabrica : GestorDependencia<NebulariumFabrica>
    {
        internal protected IServiceProvider Container { get; }
        private readonly ServiceCollection Servicos;

        public void ConfiguracaoesAdicionais(Action<IServiceProvider> configuracoesAdicionais)
        {
            configuracoesAdicionais(Container);
        }

        public NebulariumFabrica()
        {
            if (Servicos == null)
                Servicos = new ServiceCollection();

            IConfiguration configuracao = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            Servicos.AddSingleton(sp => configuracao);

            Servicos
                .AddRecursos()
                .AddValidadores()
                .AddFiltros()
                .AddServicos()
                .AddDbContexto(configuracao)
                .AddIntegrationServices(configuracao)
                .AddBarramentoEventos(configuracao)
                .AddTransient<CadastrarPessoaManipulador>();

            Container = Servicos.BuildServiceProvider();
        }



        public override object ObterInstancia(Type tipo)
        {
            return Container.GetService(tipo);
        }

        public override TInstancia ObterInstancia<TInstancia>()
        {
            return Container.GetService<TInstancia>();
        }
    }
}
