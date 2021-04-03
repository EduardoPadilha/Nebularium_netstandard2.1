using System;

namespace Nebularium.Tiamat.Excecoes
{
    public class UnicidadeExcecao : Exception
    {
        public UnicidadeExcecao(string valor) : base($"Unicidade violada: {valor}")
        {

        }
    }
}
