using Microsoft.Extensions.Configuration;
using Nebularium.Tarrasque.Configuracoes;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tarrasque.Interfaces;
using SimpleInjector;
using System;

namespace Nebularium.Tellurian
{
    public class NebulariumFabrica : GestorDependencia<NebulariumFabrica>
    {
        public NebulariumFabrica()
        {
            Container = new Container();
            IConfiguration configuracao = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            GestorAutoMapper.Inicializar();
            Container.Register(() => GestorAutoMapper.Instancia.Mapper, Lifestyle.Singleton);

            Container.Register(() => configuracao, Lifestyle.Singleton);
            Container.Register<IGestorConfiguracao, GestorConfiguracaoPadrao>(Lifestyle.Singleton);

            Container.Verify();

        }
        internal protected Container Container { get; }
        public override object ObterInstancia(Type tipo)
        {
            return Container.GetInstance(tipo);
        }

        public override TInstancia ObterInstancia<TInstancia>()
        {
            return Container.GetInstance<TInstancia>();
        }
    }
}
