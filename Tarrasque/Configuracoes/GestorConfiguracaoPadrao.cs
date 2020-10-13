using Microsoft.Extensions.Configuration;
using Nebularium.Tarrasque.Gestores;

namespace Nebularium.Tarrasque.Configuracoes
{
    public class GestorConfiguracaoPadrao : GestorConfiguracao
    {
        public GestorConfiguracaoPadrao(IConfiguration configuration) : base(configuration)
        {
        }

        public override TConfiguracao ObterConfiguracao<TConfiguracao>()
        {
            var config = new TConfiguracao();
            return configuration.GetSection(config.Secao).Get<TConfiguracao>();
        }
    }
}
