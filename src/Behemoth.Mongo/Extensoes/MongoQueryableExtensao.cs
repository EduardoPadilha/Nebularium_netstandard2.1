using MongoDB.Driver.Linq;
using Nebularium.Tiamat.Abstracoes;

namespace Nebularium.Behemoth.Mongo.Extensoes
{
    public static class MongoQueryableExtensao
    {
        public static IMongoQueryable<T> Pagina<T>(this IMongoQueryable<T> lista, IPaginador paginador)
        {
            if (paginador.TotalRegistros == 0)
                return default;
            return lista.Skip((paginador.Pagina - 1) * paginador.TamanhoPagina).Take(paginador.TamanhoPagina);
        }
    }
}
