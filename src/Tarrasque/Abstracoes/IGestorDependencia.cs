using System;

namespace Nebularium.Tarrasque.Abstracoes
{
    public interface IGestorDependencia
    {
        object ObterInstancia(Type tipo);
        TInstancia ObterInstancia<TInstancia>() where TInstancia : class;
        //IEnumerable<object> ObterInstancias(Type tipo);
        //IEnumerable<TInstancia> ObterInstancias<TInstancia>() where TInstancia : class;
    }
}
