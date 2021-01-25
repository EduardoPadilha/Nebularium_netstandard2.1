using System;

namespace Nebularium.Tiamat.Excecoes
{
    public class UnicidadeException : Exception
    {
        public UnicidadeException(string valor) : base($"Unicidade violada: {valor}")
        {

        }
    }
}
