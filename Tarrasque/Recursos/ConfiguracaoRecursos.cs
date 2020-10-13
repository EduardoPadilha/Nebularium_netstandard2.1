using System;
using System.Collections.Generic;

namespace Nebularium.Tarrasque.Recursos
{
    public class ConfiguracaoRecursos
    {
        protected static ConfiguracaoRecursos Instancia { get; } = new ConfiguracaoRecursos();
        protected ConfiguracaoRecursos()
        {
            Recursos = new List<Type>();
        }
        protected List<Type> Recursos { get; }
        public static void AdicionarRecurso(Type recurso)
        {
            Instancia.Recursos.Add(recurso);
        }
        public static List<Type> RecursosConfigurados { get { return Instancia.Recursos; } }
    }
}
