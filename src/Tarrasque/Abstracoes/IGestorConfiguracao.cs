using Microsoft.Extensions.Configuration;

namespace Nebularium.Tarrasque.Abstracoes
{
    public interface IGestorConfiguracao
    {
        IConfiguration Configuracao { get; }
        TConfiguracao ObterConfiguracao<TConfiguracao>() where TConfiguracao : IConfiguracao, new();
    }
}
