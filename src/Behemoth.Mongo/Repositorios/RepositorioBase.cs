using MongoDB.Driver;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Tarrasque.Extensoes;

namespace Nebularium.Behemoth.Mongo.Repositorios
{
    public abstract class RepositorioBase<TEntidade>
    {
        protected readonly IMongoContexto contexto;
        protected readonly IMongoCollection<TEntidade> colecao;

        protected RepositorioBase(IMongoContexto contexto)
        {
            this.contexto = contexto;
            colecao = NomeColecao.limpoNuloBrancoOuZero() ?
                contexto.ObterColecao<TEntidade>() :
                contexto.ObterColecao<TEntidade>(NomeColecao);
        }

        protected virtual string NomeColecao => null;
    }
}
