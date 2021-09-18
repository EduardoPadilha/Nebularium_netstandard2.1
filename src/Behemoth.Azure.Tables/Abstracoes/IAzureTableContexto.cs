using Microsoft.Azure.Cosmos.Table;
using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tiamat.Abstracoes;
using System.Threading.Tasks;

namespace Nebularium.Behemoth.Azure.Tables.Abstracoes
{
    public interface IAzureTableContexto : IContexto
    {
        IDbConfiguracao ObterConfiguracao { get; }
        CloudTableClient ObterCliente { get; }

        Task<CloudTable> ObterTabela<T>();
        Task<CloudTable> ObterTabela<T>(string nomeTabela);
    }
}
