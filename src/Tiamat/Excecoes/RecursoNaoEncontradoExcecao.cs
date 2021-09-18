using System;

namespace Nebularium.Tiamat.Excecoes
{
    public class RecursoNaoEncontradoExcecao : Exception
    {
        public RecursoNaoEncontradoExcecao() { }
        public RecursoNaoEncontradoExcecao(string menssagem) : base(menssagem)
        {
        }
    }
}
