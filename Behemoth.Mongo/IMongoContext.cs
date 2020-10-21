using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nebularium.Tiamat.Interfaces;

namespace Nebularium.Behemoth.Mongo
{
    public interface IMongoContext : IContext
    {
        IMongoDatabase OberDataBase { get; }
        IMongoQueryable<T> ObterColecao<T>();
    }
}
