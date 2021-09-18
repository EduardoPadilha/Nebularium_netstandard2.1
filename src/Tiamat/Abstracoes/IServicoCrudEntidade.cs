using System.Threading.Tasks;

namespace Nebularium.Tiamat.Abstracoes
{
    public interface IServicoCrudEntidade<TEntidade> : IServicoCrud<TEntidade> where TEntidade : IEntidade, new()
    {
        Task<TEntidade> ObterAsync(string id);
    }
}
