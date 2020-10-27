using Microsoft.Extensions.Configuration;
using Nebularium.Tarrasque.Interfaces;

namespace Nebularium.Tarrasque.Gestores
{
    public abstract class GestorConfiguracao : IGestorConfiguracao
    {
        protected readonly IConfiguration configuration;
        public IConfiguration Configuracao => configuration;
        protected GestorConfiguracao(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public abstract TConfiguracao ObterConfiguracao<TConfiguracao>() where TConfiguracao : IConfiguracao, new();
    }
}
