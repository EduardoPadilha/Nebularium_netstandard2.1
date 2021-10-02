using Nebularium.Tiamat.Recursos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nebularium.Tiamat.Abstracoes
{
    public interface IRepositorioEntidadeBase<TEntidade> where TEntidade : IEntidade, new()
    {
        #region Consulta
        Task<TEntidade> ObterAsync(string id);
        Task<IEnumerable<TEntidade>> ObterTodosAsync(IFiltro<TEntidade> filtro, IPaginador paginador = null);
        Task<IEnumerable<TEntidade>> ObterTodosAsync<T>(IFiltro<T> filtro, IPaginador paginador = null);
        Task<IEnumerable<TEntidade>> ObterTodosAsync(Expression<Func<TEntidade, bool>> predicado, IPaginador paginador = null);
        Task<IEnumerable<TEntidade>> ObterTodosAsync<T>(Expression<Func<T, bool>> predicado, IPaginador paginador = null);
        #endregion
        #region Comandos
        Task AdicionarAsync(TEntidade entidade);
        Task AdicionarAsync(IEnumerable<TEntidade> entidades);
        Task<bool> AtualizarUmAsync(Expression<Func<TEntidade, bool>> predicado, List<PropriedadeValor> propriedades);
        Task<bool> AtualizarMuitosAsync(Expression<Func<TEntidade, bool>> predicado, List<PropriedadeValor> propriedades);
        Task<bool> RemoverUmAsync(string id);
        Task<bool> RemoverMuitosAsync(Expression<Func<TEntidade, bool>> predicado);
        #endregion
    }
}
