using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nebularium.Tiamat.Interfaces
{
    public interface IConsultaRepositorio<TEntidade> where TEntidade : IEntidade, new()
    {
        Task<TEntidade> ObterAsync(string id);
        Task<IList<TEntidade>> ObterTodosAsync<T>(IFiltro<T> filtro);
        Task<IList<TEntidade>> ObterTodosAsync<T>(Expression<Func<T, bool>> predicado);
        Task<IList<TEntidade>> ObterTodosAsync<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> predicado);
    }
}
