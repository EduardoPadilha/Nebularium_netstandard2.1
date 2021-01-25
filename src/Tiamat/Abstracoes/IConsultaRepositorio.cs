using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nebularium.Tiamat.Abstracoes
{
    public interface IConsultaRepositorio<TEntidade> : IConsultaRepositorioBase<TEntidade> where TEntidade : IEntidade, new()
    {
        Task<TEntidade> ObterAtivoAsync(string id);
        Task<IEnumerable<TEntidade>> ObterTodosAtivosAsync(IFiltro<TEntidade> filtro, IPaginador paginador = null);
        Task<IEnumerable<TEntidade>> ObterTodosAtivosAsync<T>(IFiltro<T> filtro, IPaginador paginador = null);
        Task<IEnumerable<TEntidade>> ObterTodosAtivosAsync(Expression<Func<TEntidade, bool>> predicado, IPaginador paginador = null);
        Task<IEnumerable<TEntidade>> ObterTodosAtivosAsync<T>(Expression<Func<T, bool>> predicado, IPaginador paginador = null);
        Task<IEnumerable<TEntidade>> ObterTodosQueryableAtivosAsync(Expression<Func<IQueryable<TEntidade>, IQueryable<TEntidade>>> predicado, IPaginador paginador = null);
        Task<IEnumerable<TEntidade>> ObterTodosQueryableAtivosAsync<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> predicado, IPaginador paginador = null);
    }
}
