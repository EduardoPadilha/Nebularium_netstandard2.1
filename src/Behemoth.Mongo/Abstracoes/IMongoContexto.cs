using MongoDB.Driver;
using Nebularium.Behemoth.Abstracoes;

namespace Nebularium.Behemoth.Mongo.Abstracoes
{
    public interface IMongoContexto : IContexto
    {
        IDbConfiguracao ObterConfiguracao { get; }
        IMongoDatabase OberDataBase { get; }
        IMongoCollection<T> ObterColecao<T>();
        IMongoCollection<T> ObterColecao<T>(string nomeColecao);
    }
}
