using System.Threading.Tasks;

namespace Nebularium.Tiamat.Interfaces
{
    public interface IComandoRepositorio<TEntidade> where TEntidade : IEntidade, new()
    {
        Task<TEntidade> Adicionar(TEntidade entidade);
        Task Atualizar(TEntidade entidade);
        Task Remover(TEntidade entidade);
    }
}
