using System.Threading.Tasks;

namespace Nebularium.Tiamat.Interfaces
{
    public interface IConsultaRepositorio<TEntidade> where TEntidade : IEntidade, new()
    {
        Task<TEntidade> Obter(string id);
        //Task<IList<TEntidade>> Obter(TEntidade filtro);
    }
}
