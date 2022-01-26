using Microsoft.Azure.Cosmos.Table;
using Nebularium.Behemoth.Azure.Tables.Abstracoes;
using Nebularium.Tarrasque.Extensoes;
using System.Threading.Tasks;

namespace Nebularium.Behemoth.Azure.Tables.Repositorios
{
    public abstract class TabelarRepositorioBase<TEntidade>
    {
        protected readonly IAzureTableContexto contexto;
        protected readonly CloudTable tabela;

        protected TabelarRepositorioBase(IAzureTableContexto contexto)
        {
            this.contexto = contexto;
            var tarefa = NomeTabela.limpoNuloBrancoOuZero() ?
                contexto.ObterTabela<TEntidade>() :
                contexto.ObterTabela<TEntidade>(NomeTabela);
            Task.WaitAll(tarefa);
            tabela = tarefa.Result;
        }

        protected virtual string NomeTabela => null;
    }
}
