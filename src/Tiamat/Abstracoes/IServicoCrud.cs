using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nebularium.Tiamat.Abstracoes
{
    public interface IServicoCrud<TEntidade>
    {
        Task<IEnumerable<TEntidade>> ObterTodosAsync(IFiltro<TEntidade> filtro, IPaginacao paginacao = null);
        Task<TEntidade> AdicionarAsync(TEntidade entidade);
        Task<bool> AtualizarAsync(TEntidade entidade);
        Task<bool> RemoverAsync(TEntidade entidade);
    }
}
