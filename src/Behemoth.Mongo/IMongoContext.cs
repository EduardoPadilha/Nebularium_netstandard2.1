using MongoDB.Driver;
using Nebularium.Tiamat.Interfaces;

namespace Nebularium.Behemoth.Mongo
{
    public interface IMongoContext : IContext
    {
        IMongoDatabase OberDataBase { get; }
        IMongoCollection<T> ObterColecao<T>();
    }
}
