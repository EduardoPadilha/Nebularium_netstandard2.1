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

        Task<IEnumerable<TEntidade>> ObterTodosAsync(IFiltro<TEntidade> filtro);
        Task<IEnumerable<TEntidade>> ObterTodosAsync<T>(IFiltro<T> filtro);

        Task<IEnumerable<TEntidade>> ObterTodosAsync(Expression<Func<TEntidade, bool>> predicado);
        Task<IEnumerable<TEntidade>> ObterTodosAsync<T>(Expression<Func<T, bool>> predicado);

        Task<IEnumerable<TEntidade>> ObterTodosAsync<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> predicado);
        Task<IEnumerable<TEntidade>> ObterTodosAsync(Expression<Func<IQueryable<TEntidade>, IQueryable<TEntidade>>> predicado);
    }
}
