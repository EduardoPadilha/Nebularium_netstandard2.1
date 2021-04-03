using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nebularium.Behemoth.Mongo.Repositorios
{
    public abstract class ConsultaRepositorio<TEntidade> : ConsultaRepositorioBase<TEntidade>,
        IConsultaRepositorio<TEntidade>
         where TEntidade : Entidade, new()
    {
        protected ConsultaRepositorio(IMongoContexto contexto) : base(contexto)
        {
        }

        public override IOrderedMongoQueryable<TEntidade> OrdernarPadrao(IMongoQueryable<TEntidade> query)
        {
            return query.OrderBy(c => c.Metadado.DataCriacao);
        }
        protected override IMongoQueryable<TEntidade> ObterTodos()
        {
            return OrdernarPadrao(colecao.AsQueryable().Where(c => !c.Metadado.DataDelecao.HasValue));
        }
        protected virtual IMongoQueryable<TEntidade> ObterTodosAtivos()
        {
            return ObterTodos().Where(c => c.Metadado.Ativo);
        }
        protected virtual IMongoQueryable<TEntidade> ObterTodosAtivos<T>(IFiltro<T> filtro)
        {
            return ObterTodosAtivos().Where(filtro.ObterPredicados().ConvertePredicado<T, TEntidade>());
        }

        #region Implementação IConsultaRepositorio
        public virtual Task<TEntidade> ObterAtivoAsync(string id)
        {
            return ObterTodosAtivos().FirstOrDefaultAsync(c => c.Id == id);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAtivosAsync(IFiltro<TEntidade> filtro, IPaginador paginador = null)
        {
            return ObterTodosAtivosAsync<TEntidade>(filtro, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAtivosAsync<T>(IFiltro<T> filtro, IPaginador paginador = null)
        {
            var query = ObterTodosAtivos(filtro);
            return ProcessarBuscas(query, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAtivosAsync(Expression<Func<TEntidade, bool>> predicado, IPaginador paginador = null)
        {
            return ObterTodosAtivosAsync<TEntidade>(predicado, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosAtivosAsync<T>(Expression<Func<T, bool>> predicado, IPaginador paginador = null)
        {
            var query = ObterTodosAtivos().Where(predicado.ConvertePredicado<T, TEntidade>());
            return ProcessarBuscas(query, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosQueryableAtivosAsync(Expression<Func<IQueryable<TEntidade>, IQueryable<TEntidade>>> predicado, IPaginador paginador = null)
        {
            return ObterTodosQueryableAtivosAsync<TEntidade>(predicado, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosQueryableAtivosAsync<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> predicado, IPaginador paginador = null)
        {
            var predicadoConvertido = predicado.ConvertePredicado<T, TEntidade>();
            var query = (IMongoQueryable<TEntidade>)predicadoConvertido.Compile()(ObterTodosAtivos());
            return ProcessarBuscas(query, paginador);
        }
        #endregion
    }
}
