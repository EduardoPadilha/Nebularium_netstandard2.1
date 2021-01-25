using Nebularium.Tiamat.Abstracoes;

namespace Nebularium.Tiamat.Paginadores
{
    public class Paginacao : IPaginacao
    {
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
    }
}
