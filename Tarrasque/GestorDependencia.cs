using Nebularium.Tarrasque.Interfaces;
using System;

namespace Nebularium.Tarrasque
{
    public abstract class GestorDependencia : IGestorDependencia
    {
        public static IGestorDependencia Instancia { get; protected set; }

        public abstract object ObterInstancia(Type tipo);

        public abstract TInstancia ObterInstancia<TInstancia>() where TInstancia : class;

        //public abstract IEnumerable<object> ObterInstancias(Type tipo);

        //public abstract IEnumerable<TInstancia> ObterInstancias<TInstancia>() where TInstancia : class;
    }

    public abstract class GestorDependencia<T> : GestorDependencia where T : IGestorDependencia, new()
    {
        public static void Inicializar()
        {
            if (Instancia != null) return;

            Instancia = new T();
        }

        protected GestorDependencia()
        {
        }
    }
}
