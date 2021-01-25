using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Enumeracoes;

namespace Nebularium.Tiamat.Entidades.Resultados
{
    public class MetadadoResultado : IMetadadoResultado
    {
        public MetadadoResultado(TipoResultado tipo)
        {
            Tipo = tipo.ToString();
        }
        public string Tipo { get; }
    }

    public class MetadadoPaginado : MetadadoResultado
    {
        public MetadadoPaginado(IPaginador paginador) : base(TipoResultado.List)
        {
            Pagina = paginador.Pagina;
            TamanhoPagina = paginador.TamanhoPagina;
            TotalPaginas = paginador.TotalPaginas;
            Total = paginador.TotalRegistros;
        }

        public int Pagina { get; }
        public int TamanhoPagina { get; }
        public int TotalPaginas { get; }
        public long Total { get; }
    }
}
