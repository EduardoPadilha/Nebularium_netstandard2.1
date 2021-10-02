using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tiamat.Abstracoes;
using System.Threading.Tasks;

namespace Nebularium.Tellurian.Drone.Interfaces.Repositorios
{
    public interface IPessoaRepositorio : IRepositorioEntidade<Pessoa>
    {
        Task AtualizarNaMao();
    }
}
