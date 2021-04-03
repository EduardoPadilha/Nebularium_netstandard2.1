using Nebularium.Tiamat.Validacoes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nebularium.Tiamat.Excecoes
{
    public class ValidacaoExcecao : Exception
    {
        private readonly List<ErroValidacao> Erros;
        public ValidacaoExcecao(List<ErroValidacao> erros) : base("Erros na validação")
        {
            Erros = erros;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Erros.Select(erro => erro.ToString()));
        }
    }
}
