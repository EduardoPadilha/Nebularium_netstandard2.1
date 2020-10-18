using System.Threading.Tasks;

namespace Nebularium.Tiamat.Interfaces
{
    public interface IComandoRepositorio<TEntidade> where TEntidade : IEntidade, new()
    {
        Task<TEntidade> AdicionarAsync(TEntidade entidade);
        Task AtualizarAsync(TEntidade entidade);
        Task RemoverAsync(TEntidade entidade);
    }
}
