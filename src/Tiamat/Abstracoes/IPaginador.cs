using System;
using System.Collections.Generic;
using System.Linq;

namespace Nebularium.Tiamat.Abstracoes
{
    public interface IPaginador
    {
        int Pagina { get; set; }
        int TotalPaginas { get; set; }
        int TamanhoPagina { get; set; }
        long TotalRegistros { get; set; }
    }

    public static class PaginadorExtensao
    {
        public static void IniciaPaginador(this IPaginador paginador, long totalRegistros)
        {
            paginador.TotalRegistros = totalRegistros;

            if (paginador.Pagina == 0)
                paginador.Pagina = 1;

            if (paginador.TamanhoPagina == 0)
                paginador.TamanhoPagina = 10;

            paginador.TotalPaginas = paginador.TotalRegistros == 0 ? 1 : (int)Math.Ceiling((decimal)paginador.TotalRegistros / paginador.TamanhoPagina);
            if (paginador.Pagina > paginador.TotalPaginas)
                paginador.Pagina = paginador.TotalPaginas;
        }

        public static IEnumerable<T> Pagina<T>(this IEnumerable<T> lista, IPaginador paginador)
        {
            if (paginador.TotalRegistros == 0)
                return default;
            return lista.Skip((paginador.Pagina - 1) * paginador.TamanhoPagina).Take(paginador.TamanhoPagina);
        }
    }
}
