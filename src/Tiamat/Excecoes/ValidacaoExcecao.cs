using Nebularium.Tiamat.Validacoes;
using System;
using System.Collections.Generic;

namespace Nebularium.Tiamat.Excecoes
{
    public class ValidacaoExcecao : Exception
    {
        public List<ErroValidacao> Erros { get; protected set; }

        public ValidacaoExcecao(List<ErroValidacao> erros)
        {
            Erros = erros;
        }

        public ValidacaoExcecao(List<ValidacaoSimples> erros)
        {
            Erros = erros.ConvertAll(c => new ErroValidacao(c.Campo, c.Mensagem));
        }
    }
}
