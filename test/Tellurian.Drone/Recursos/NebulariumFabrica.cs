using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tarrasque.Recursos;
using Nebularium.Tellurian.Drone.Manipuladores;
using Nebularium.Tiamat.Recursos;
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
                .AddContextoNotificacao()
                .AddRepositoriosAutoMapper()
                .AddRepositorios()
                .AddRecursos()
                .AddFeatures()
                .AddComandos()
                .AddGestorDependenciaAspnetPadrao()
                .AddIntegrationServices(configuracao)
                .AddBarramentoEventos(configuracao)
                .AddTransient<CadastrarPessoaManipulador>();

            Container = Servicos.BuildServiceProvider();//(new ServiceProviderOptions { ValidateOnBuild = true });
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
