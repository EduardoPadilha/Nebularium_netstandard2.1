using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nebularium.Tiamat.Abstracoes
{
    public interface ITabelarServico<TEntidade>
    {
        Task<TEntidade> ObterAsync(string chaveParticao, string chaveLinha);
        Task<IEnumerable<TEntidade>> ObterTodosAsync(string chaveParticao, IPaginacao paginacao = null);
        Task<TEntidade> AdicionarAsync(TEntidade entidade);
        Task<bool> AtualizarAsync(TEntidade entidade);
        Task<bool> RemoverAsync(TEntidade entidade);
    }
}
