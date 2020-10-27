using Microsoft.Extensions.Configuration;

namespace Nebularium.Tarrasque.Interfaces
{
    public interface IGestorConfiguracao
    {
        IConfiguration Configuracao { get; }
        TConfiguracao ObterConfiguracao<TConfiguracao>() where TConfiguracao : IConfiguracao, new();
    }
}
