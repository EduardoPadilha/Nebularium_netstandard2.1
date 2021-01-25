using System;
using System.Linq;
using System.Linq.Expressions;

namespace Nebularium.Tiamat.Abstracoes
{
    public interface IFiltro<T>
    {
        IFiltroOpcoes<T> AdicionarRegra(Expression<Func<T, bool>> criterio);
        void SetarModeloCriterio(T criterio);
        IQueryable<T> ObterFiltro(IQueryable<T> lista);
        Expression<Func<T, bool>> ObterPredicados();
    }

    public interface IFiltroOpcoes<T>
    {
        void SobCondicional(Expression<Func<T, bool>> condicao);
    }
}
