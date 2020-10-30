using System.Threading.Tasks;

namespace Nebularium.Weaver.Interfaces
{
    public interface IEventoManipulador<TEvento> where TEvento : IEvento
    {
        Task Resolve(TEvento evento);
    }
}
