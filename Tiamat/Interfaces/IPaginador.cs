namespace Nebularium.Tiamat.Interfaces
{
    public interface IPaginador
    {
        int Pagina { get; set; }
        int TotalPaginas { get; set; }
        int RegistrosPorPagina { get; set; }
        int TotalRegistros { get; set; }
    }
}
