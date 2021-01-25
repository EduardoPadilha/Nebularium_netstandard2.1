using MongoDB.Driver;
using Nebularium.Tiamat.Abstracoes;

namespace Nebularium.Behemoth.Mongo.Abstracoes
{
    public interface IMongoContexto : IContexto
    {
        IMongoDatabase OberDataBase { get; }
        IMongoCollection<T> ObterColecao<T>();
    }
}
