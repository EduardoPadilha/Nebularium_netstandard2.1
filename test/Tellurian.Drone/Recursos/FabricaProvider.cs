using Microsoft.Extensions.DependencyInjection;
using Nebularium.Tarrasque.Gestores;
using System;

namespace Nebularium.Tellurian.Drone.Recursos
{
    public class FabricaProvider : GestorDependencia<FabricaProvider>
    {
        internal protected IServiceProvider Container { get; private set; }

        public FabricaProvider()
        {
        }

        public void AddContainer(IServiceProvider container)
        {
            Container = container;
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
