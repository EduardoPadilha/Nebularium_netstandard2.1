using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Enumeracoes;

namespace Nebularium.Tiamat.Entidades.Resultados
{
    public class MetadadoResultado : IMetadadoResultado
    {
        public MetadadoResultado() { }
        public MetadadoResultado(TipoResultado tipo)
        {
            Tipo = tipo.ToString();
        }
        public string Tipo { get; set; }
    }

    public class MetadadoPaginado : MetadadoResultado
    {
        public MetadadoPaginado() { }
        public MetadadoPaginado(IPaginador paginador) : base(TipoResultado.List)
        {
            Pagina = paginador.Pagina;
            TamanhoPagina = paginador.TamanhoPagina;
            TotalPaginas = paginador.TotalPaginas;
            Total = paginador.TotalRegistros;
        }

        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalPaginas { get; set; }
        public long Total { get; set; }
    }
}
