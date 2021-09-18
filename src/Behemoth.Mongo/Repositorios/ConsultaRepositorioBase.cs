using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nebularium.Behemoth.Mongo.Repositorios
{
    public abstract class ConsultaRepositorioBase<TEntidade> : RepositorioBase<TEntidade>, IConsultaRepositorioBase<TEntidade>
        where TEntidade : IEntidade, new()
    {
        public ConsultaRepositorioBase(IMongoContexto contexto) : base(contexto)
        {
        }

        #region Implementação IConsultaRepositorioBase
        public virtual Task<TEntidade> ObterAsync(string id)
        {
            return ObterTodos(c => c.Id == id).FirstOrDefaultAsync();
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAsync(IFiltro<TEntidade> filtro, IPaginador paginador = null)
        {
            return ObterTodosAsync<TEntidade>(filtro, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAsync<T>(IFiltro<T> filtro, IPaginador paginador = null)
        {
            var query = ObterFitro(filtro);
            return ProcessarBuscas(query, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAsync(Expression<Func<TEntidade, bool>> predicado, IPaginador paginador = null)
        {
            return ObterTodosAsync<TEntidade>(predicado, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAsync<T>(Expression<Func<T, bool>> predicado, IPaginador paginador = null)
        {
            var query = ObterTodos(ConvertePredicadosSeNecessario(predicado));
            return ProcessarBuscas(query, paginador);
        }
        #endregion

        #region Implementação de suporte pro repositório

        private IFindFluent<TEntidade, TEntidade> Pagina(IFindFluent<TEntidade, TEntidade> query, IPaginador paginador)
        {
            if (paginador.TotalRegistros == 0)
                return default;
            return query.Skip((paginador.Pagina - 1) * paginador.TamanhoPagina).Limit(paginador.TamanhoPagina);
        }

        protected async Task<IEnumerable<TEntidade>> ProcessarBuscas(IFindFluent<TEntidade, TEntidade> query, IPaginador paginador)
        {
            if (paginador == null)
                return await OrdenacaoPadrao(query).ToListAsync();

            var total = await query.CountDocumentsAsync();

            paginador.IniciaPaginador(total);
            var queryOrdenada = OrdenacaoPadrao(query);
            var queryPaginada = Pagina(queryOrdenada, paginador);
            if (queryPaginada == default)
                return default;

            return await queryPaginada.ToListAsync();
        }
        public virtual IFindFluent<TEntidade, TEntidade> OrdenacaoPadrao(IFindFluent<TEntidade, TEntidade> query)
        {
            return query.SortBy(c => c.Id);
        }
        protected virtual IFindFluent<TEntidade, TEntidade> ObterFitro<T>(IFiltro<T> filtro)
        {
            var predicado = ConvertePredicadosSeNecessario(filtro.ObterPredicados());
            return ObterTodos(predicado);
        }

        protected virtual IFindFluent<TEntidade, TEntidade> ObterTodos(Expression<Func<TEntidade, bool>> predicado)
        {
            return colecao.Find(predicado);
        }

        protected Expression<Func<TEntidade, bool>> ConvertePredicadosSeNecessario<T>(Expression<Func<T, bool>> predicado)
        {
            return typeof(T) == typeof(TEntidade) ? predicado as Expression<Func<TEntidade, bool>> : predicado.ConvertePredicado<T, TEntidade>();
        }
        #endregion
    }
}
