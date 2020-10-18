using Nebularium.Tiamat.Interfaces;
using System.Linq;

namespace Nebularium.Behemoth.Mongo
{
    public interface IMongoContext : IContext
    {
        IQueryable<T> ObterColecao<T>();
    }
}
