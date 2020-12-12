using System.Threading.Tasks;

namespace Nebularium.Weaver.Interfaces
{
    public interface IManipuladorEvento<TEvento> where TEvento : IEvento
    {
        Task Resolver(TEvento evento);
    }
}
