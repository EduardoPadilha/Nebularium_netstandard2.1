using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Behemoth.Mongo.Extensoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nebularium.Behemoth.Mongo.Repositorios
{
    public abstract class ConsultaRepositorioBase<TEntidade> : IConsultaRepositorioBase<TEntidade>
        where TEntidade : IEntidade, new()
    {
        protected readonly IMongoContexto contexto;
        protected readonly IMongoCollection<TEntidade> colecao;
        public ConsultaRepositorioBase(IMongoContexto contexto)
        {
            this.contexto = contexto;
            colecao = contexto.ObterColecao<TEntidade>();
        }

        #region Implementação IConsultaRepositorio
        public virtual Task<TEntidade> ObterAsync(string id)
        {
            return ObterTodos().FirstOrDefaultAsync(c => c.Id == id);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAsync(IFiltro<TEntidade> filtro, IPaginador paginador = null)
        {
            return ObterTodosAsync<TEntidade>(filtro, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAsync<T>(IFiltro<T> filtro, IPaginador paginador = null)
        {
            var query = ObterTodos(filtro);
            return ProcessarBuscas(query, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAsync(Expression<Func<TEntidade, bool>> predicado, IPaginador paginador = null)
        {
            return ObterTodosAsync<TEntidade>(predicado, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAsync<T>(Expression<Func<T, bool>> predicado, IPaginador paginador = null)
        {
            var query = ObterTodos().Where(predicado.ConvertePredicado<T, TEntidade>());
            return ProcessarBuscas(query, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosQueryableAsync(Expression<Func<IQueryable<TEntidade>, IQueryable<TEntidade>>> predicado, IPaginador paginador = null)
        {
            return ObterTodosQueryableAsync<TEntidade>(predicado, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosQueryableAsync<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> predicado, IPaginador paginador = null)
        {
            var predicadoConvertido = predicado.ConvertePredicado<T, TEntidade>();
            var query = (IMongoQueryable<TEntidade>)predicadoConvertido.Compile()(ObterTodos());
            return ProcessarBuscas(query, paginador);
        }
        #endregion

        #region Implementação de suporte pro repositório

        protected async Task<IEnumerable<TEntidade>> ProcessarBuscas(IMongoQueryable<TEntidade> query, IPaginador paginador)
        {
            if (paginador == null)
                return await query.ToListAsync();

            var total = await query.LongCountAsync();

            paginador.IniciaPaginador(total);

            var queryPaginada = query.Pagina(paginador);
            if (queryPaginada == default)
                return default;

            return await queryPaginada.ToListAsync();
        }
        public virtual IOrderedMongoQueryable<TEntidade> OrdernarPadrao(IMongoQueryable<TEntidade> query)
        {
            return query.OrderBy(z => z.Id);
        }
        protected virtual IMongoQueryable<TEntidade> ObterTodos<T>(IFiltro<T> filtro)
        {
            return ObterTodos().Where(filtro.ObterPredicados().ConvertePredicado<T, TEntidade>());
        }
        protected virtual IMongoQueryable<TEntidade> ObterTodos()
        {
            return OrdernarPadrao(colecao.AsQueryable());
        }
        #endregion
    }
    public abstract class ConsultaRepositorioBase<TEntidade, TProxy> : IConsultaRepositorioBase<TEntidade>
        where TEntidade : IEntidade, new()
        where TProxy : IEntidade, new()
    {
        protected readonly IMongoContexto contexto;
        protected readonly IMongoCollection<TProxy> colecao;
        public ConsultaRepositorioBase(IMongoContexto contexto)
        {
            this.contexto = contexto;
            colecao = contexto.ObterColecao<TProxy>();
        }

        #region Implementação IConsultaRepositorio
        public virtual Task<TEntidade> ObterAsync(string id)
        {
            var resultado = ObterTodos().FirstOrDefaultAsync(c => c.Id == id);
            return resultado.ComoAsync<TProxy, TEntidade>();
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAsync(IFiltro<TEntidade> filtro, IPaginador paginador = null)
        {
            return ObterTodosAsync<TEntidade>(filtro, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAsync<T>(IFiltro<T> filtro, IPaginador paginador = null)
        {
            var query = ObterTodos(filtro);
            return ProcessarBuscas(query, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAsync(Expression<Func<TEntidade, bool>> predicado, IPaginador paginador = null)
        {
            return ObterTodosAsync<TEntidade>(predicado, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAsync<T>(Expression<Func<T, bool>> predicado, IPaginador paginador = null)
        {
            var query = ObterTodos().Where(predicado.ConvertePredicado<T, TProxy>());
            return ProcessarBuscas(query, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosQueryableAsync(Expression<Func<IQueryable<TEntidade>, IQueryable<TEntidade>>> predicado, IPaginador paginador = null)
        {
            return ObterTodosQueryableAsync<TEntidade>(predicado, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosQueryableAsync<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> predicado, IPaginador paginador = null)
        {
            var predicadoConvertido = predicado.ConvertePredicado<T, TProxy>();
            var query = (IMongoQueryable<TProxy>)predicadoConvertido.Compile()(ObterTodos());
            return ProcessarBuscas(query, paginador);
        }
        #endregion

        #region Implementação de suporte pro repositório

        protected async Task<IEnumerable<TEntidade>> ProcessarBuscas(IMongoQueryable<TProxy> query, IPaginador paginador)
        {
            if (paginador == null)
                return await query.ToListAsync().ComoAsync<List<TProxy>, IEnumerable<TEntidade>>();

            var total = await query.LongCountAsync();

            paginador.IniciaPaginador(total);

            var queryPaginada = query.Pagina(paginador);
            if (queryPaginada == default)
                return default;

            return await queryPaginada.ToListAsync().ComoAsync<List<TProxy>, IEnumerable<TEntidade>>();
        }
        public virtual IOrderedMongoQueryable<TProxy> OrdernarPadrao(IMongoQueryable<TProxy> query)
        {
            return query.OrderBy(z => z.Id);
        }
        protected virtual IMongoQueryable<TProxy> ObterTodos<T>(IFiltro<T> filtro)
        {
            return ObterTodos().Where(filtro.ObterPredicados().ConvertePredicado<T, TProxy>());
        }
        protected virtual IMongoQueryable<TProxy> ObterTodos()
        {
            return OrdernarPadrao(colecao.AsQueryable());
        }
        #endregion
    }
}
