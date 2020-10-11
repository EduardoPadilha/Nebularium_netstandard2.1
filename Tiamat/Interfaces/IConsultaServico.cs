using System.Threading.Tasks;

namespace Nebularium.Tiamat.Interfaces
{
    public interface IConsultaServico<TEntidade> where TEntidade : IEntidade, new()
    {
        Task<TEntidade> Obter(string id);
        //Task<IList<TEntidade>> Obter(TEntidade filtro);
    }
}
