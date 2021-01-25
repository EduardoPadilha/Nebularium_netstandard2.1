using Microsoft.Extensions.DependencyInjection;
using System;

namespace Nebularium.Tarrasque.Gestores
{
    public class AspnetGestorPadrao : GestorDependencia<AspnetGestorPadrao>
    {
        internal protected IServiceProvider Container { get; }
        public AspnetGestorPadrao()
        {
        }
        public AspnetGestorPadrao(IServiceCollection servicos)
        {
            Container = servicos.BuildServiceProvider();
        }

        public void ConfiguracaoesAdicionais(Action<IServiceProvider> configuracoesAdicionais)
        {
            configuracoesAdicionais(Container);
        }


        public static void Inicializar(IServiceCollection servicos)
        {
            if (Instancia != null) return;

            Instancia = new AspnetGestorPadrao(servicos);
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
