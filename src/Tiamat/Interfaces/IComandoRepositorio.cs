using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nebularium.Tiamat.Interfaces
{
    public interface IComandoRepositorio<TEntidade> where TEntidade : IEntidade, new()
    {
        Task AdicionarAsync(TEntidade entidade);
        Task AdicionarAsync(IEnumerable<TEntidade> entidades);
        Task AtualizarAsync(TEntidade entidade);
        //Task AtualizarAsync(IEnumerable<TEntidade> entidades);
        Task RemoverAsync(TEntidade entidade);
        Task RemoverAsync(IEnumerable<TEntidade> entidades);
    }
}
