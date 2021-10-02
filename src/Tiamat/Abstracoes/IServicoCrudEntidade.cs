using Nebularium.Tiamat.Entidades;
using System.Threading.Tasks;

namespace Nebularium.Tiamat.Abstracoes
{
    public interface IServicoCrudEntidade<TEntidade> : IServicoCrud<TEntidade> where TEntidade : Entidade, new()
    {
        Task<TEntidade> ObterAsync(string id);
        Task<bool> AtivarDesativarAsync(TEntidade entidade, bool ativar);
    }
}
