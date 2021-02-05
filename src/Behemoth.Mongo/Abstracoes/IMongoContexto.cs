using MongoDB.Driver;
using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tiamat.Abstracoes;

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
