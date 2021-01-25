using System.Threading.Tasks;

namespace Nebularium.Tiamat.Abstracoes
{
    public interface IComandoRepositorio<TEntidade> : IComandoRepositorioBase<TEntidade> where TEntidade : IEntidade, new()
    {
        Task<bool> AtivarDesativarUmAsync(string id, bool ativar);
    }
}
