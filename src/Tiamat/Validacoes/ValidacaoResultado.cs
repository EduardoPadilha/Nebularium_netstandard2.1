using System.Collections.Generic;

namespace Nebularium.Tiamat.Validacoes
{
    public class ValidacaoResultado
    {
        public List<ErroValidacao> Erros { get; set; }
        public bool Valido { get { return Erros == null || Erros.Count == 0; } }
    }
}
