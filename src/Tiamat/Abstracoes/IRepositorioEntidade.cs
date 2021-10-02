using System.Threading.Tasks;

namespace Nebularium.Tiamat.Abstracoes
{
    public interface IRepositorioEntidade<TEntidade> : IRepositorioEntidadeBase<TEntidade> where TEntidade : IEntidade, new()
    {
        Task<bool> AtivarDesativarUmAsync(string id, bool ativar);
    }
}
