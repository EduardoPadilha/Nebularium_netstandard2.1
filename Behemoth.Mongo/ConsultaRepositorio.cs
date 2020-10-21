using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nebularium.Behemoth.Mongo
{
    public abstract class ConsultaRepositorio<TEntidade, TProxy> : IConsultaRepositorio<TEntidade>
        where TEntidade : IEntidade, new()
        where TProxy : IEntidade, new()
    {
        protected IMongoContext context { get; }
        public ConsultaRepositorio(IMongoContext context)
        {
            this.context = context;
        }

        #region Implementação IConsultaRepositorio
        public Task<TEntidade> ObterAsync(string id)
        {
            var resultado = context.ObterColecao<TProxy>().FirstOrDefaultAsync(c => c.Id == id);
            return resultado.ComoAsync<TProxy, TEntidade>();
        }
        public Task<IList<TEntidade>> ObterTodosAsync<T>(IFiltro<T> filtro)
        {
            var resultado = ObterTodos(filtro);
            return resultado.ToListAsync().ComoAsync<List<TProxy>, IList<TEntidade>>();
        }
        public Task<IList<TEntidade>> ObterTodosAsync<T>(Expression<Func<T, bool>> predicado)
        {
            var resultado = ObterTodos().Where(ConvertePredicado(predicado));
            return resultado.ToListAsync().ComoAsync<List<TProxy>, IList<TEntidade>>();
        }
        public Task<IList<TEntidade>> ObterTodosAsync<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> predicado)
        {
            var predicadoConvertido = ConvertePredicado(predicado);
            var query = (IMongoQueryable<TProxy>)predicadoConvertido.Compile()(ObterTodos());
            return query.ToListAsync().ComoAsync<List<TProxy>, IList<TEntidade>>();
        }
        #endregion

        #region Implementação de suporte pro repositório
        protected IMongoQueryable<TProxy> ObterTodos<T>(IFiltro<T> filtro)
        {
            return ObterTodos().Where(ConvertePredicado(filtro.ObterPredicados()));
        }
        protected IMongoQueryable<TProxy> ObterTodos()
        {
            return context.ObterColecao<TProxy>();
        }
        private Expression<Func<TProxy, bool>> ConvertePredicado<T>(Expression<Func<T, bool>> predicado)
        {
            return predicado.Como<Expression<Func<TProxy, bool>>>();
        }
        private Expression<Func<IQueryable<TProxy>, IQueryable<TProxy>>> ConvertePredicado<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> predicado)
        {
            return predicado.Como<Expression<Func<IQueryable<TProxy>, IQueryable<TProxy>>>>();
        }
        #endregion
    }
}
