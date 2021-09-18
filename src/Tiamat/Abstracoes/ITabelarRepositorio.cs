using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nebularium.Tiamat.Abstracoes
{
    public interface ITabelarRepositorio<TEntidade>
    {
        Task<TEntidade> ObterAsync(string chaveParticao, string chaveLinha);
        Task<IEnumerable<TEntidade>> ObterTodosAsync(string chaveParticao, IPaginador paginador = null);
        Task AdicionarAsync(TEntidade entidade);
        Task<bool> AtualizarAsync(TEntidade entidade);
        Task<bool> RemoverAsync(TEntidade entidade);
    }
}
