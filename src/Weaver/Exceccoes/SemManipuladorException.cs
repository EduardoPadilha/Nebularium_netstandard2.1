using System;

namespace Nebularium.Weaver.Exceccoes
{
    public class SemManipuladorException : Exception
    {
        public SemManipuladorException(string manipulador) : base($"Sem manipuladores para o evento {manipulador}")
        {
        }

        public SemManipuladorException(Type tipoManipulador) : this(tipoManipulador.GetInterfaces()[0].GetGenericArguments()[0].Name)
        {
        }
    }

    public class SemManipuladorException<TManipulador> : SemManipuladorException
    {
        public SemManipuladorException() : base(typeof(TManipulador).GetGenericArguments()[0].Name)
        {
        }
    }
}
