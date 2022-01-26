using Microsoft.AspNetCore.Http;
using Nebularium.Tarrasque.Extensoes;
using System.Web;

namespace Nebularium.Cthulhu.Extensoes
{
    public static class QueryCollectionExtensao
    {
        public static T Obter<T>(this IQueryCollection query, string param)
        {
            var objString = query[param];
            string obj = HttpUtility.UrlDecode(objString);
            return obj.Como<T>();
        }
    }
}
