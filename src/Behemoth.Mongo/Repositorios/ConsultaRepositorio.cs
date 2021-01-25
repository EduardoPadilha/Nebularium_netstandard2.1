using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Behemoth.Mongo.Mapeamento;
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
    public abstract class ConsultaRepositorio<TEntidade, TProxy> : ConsultaRepositorioBase<TEntidade, TProxy>,
        IConsultaRepositorio<TEntidade>
         where TEntidade : Entidade, new()
        where TProxy : EntidadeMapeamento, new()
    {
        protected ConsultaRepositorio(IMongoContexto contexto) : base(contexto)
        {
        }

        public override IOrderedMongoQueryable<TProxy> OrdernarPadrao(IMongoQueryable<TProxy> query)
        {
            return query.OrderBy(c => c.Metadado.DataCriacao);
        }
        protected override IMongoQueryable<TProxy> ObterTodos()
        {
            return OrdernarPadrao(colecao.AsQueryable().Where(c => !c.Metadado.DataDelecao.HasValue));
        }
        protected virtual IMongoQueryable<TProxy> ObterTodosAtivos()
        {
            return ObterTodos().Where(c => c.Metadado.Ativo);
        }
        protected virtual IMongoQueryable<TProxy> ObterTodosAtivos<T>(IFiltro<T> filtro)
        {
            return ObterTodosAtivos().Where(filtro.ObterPredicados().ConvertePredicado<T, TProxy>());
        }

        #region Implementação IConsultaRepositorio
        public virtual Task<TEntidade> ObterAtivoAsync(string id)
        {
            var resultado = ObterTodosAtivos().FirstOrDefaultAsync(c => c.Id == id);
            return resultado.ComoAsync<TProxy, TEntidade>();
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
            var query = ObterTodosAtivos().Where(predicado.ConvertePredicado<T, TProxy>());
            return ProcessarBuscas(query, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosQueryableAtivosAsync(Expression<Func<IQueryable<TEntidade>, IQueryable<TEntidade>>> predicado, IPaginador paginador = null)
        {
            return ObterTodosQueryableAtivosAsync<TEntidade>(predicado, paginador);
        }
        public virtual Task<IEnumerable<TEntidade>> ObterTodosQueryableAtivosAsync<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> predicado, IPaginador paginador = null)
        {
            var predicadoConvertido = predicado.ConvertePredicado<T, TProxy>();
            var query = (IMongoQueryable<TProxy>)predicadoConvertido.Compile()(ObterTodosAtivos());
            return ProcessarBuscas(query, paginador);
        }
        #endregion
    }
}
