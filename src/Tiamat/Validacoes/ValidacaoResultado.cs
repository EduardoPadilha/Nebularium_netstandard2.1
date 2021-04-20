using System.Collections.Generic;

namespace Nebularium.Tiamat.Validacoes
{
    public class ValidacaoResultado
    {
        internal static ValidacaoResultado ResultadoValido => new ValidacaoResultado(new List<ErroValidacao>());
        public ValidacaoResultado(List<ErroValidacao> erros)
        {
            Erros = erros;
        }

        public List<ErroValidacao> Erros { get; set; }
        public bool Valido { get { return Erros == null || Erros.Count == 0; } }
    }
}
