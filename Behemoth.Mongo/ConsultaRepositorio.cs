using AutoMapper;
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
        private readonly IMapper _mapper;
        public ConsultaRepositorio(IMongoContext context, IMapper mapper)
        {
            this.context = context;
            this._mapper = mapper;
        }

        #region Implementação IConsultaRepositorio
        public Task<TEntidade> ObterAsync(string id)
        {
            var resultado = context.ObterColecao<TProxy>().FirstOrDefault(c => c.Id == id);
            return Task.FromResult(resultado.Como<TEntidade>());
        }
        public Task<IList<TEntidade>> ObterTodosAsync<T>(IFiltro<T> filtro)
        {
            var resultado = ObterTodos(filtro).ToList();
            return Task.FromResult(resultado.Como<IList<TEntidade>>());
        }
        public Task<IList<TEntidade>> ObterTodosAsync<T>(Expression<Func<T, bool>> predicado)
        {
            var resultado = ObterTodos().Where(ConvertePredicado(predicado));
            return Task.FromResult(resultado.Como<IList<TEntidade>>());
        }
        public Task<IList<TEntidade>> ObterTodosAsync<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> predicado)
        {
            var predicadoConvertido = ConvertePredicado(predicado);
            var resultado = predicadoConvertido.Compile().Invoke(ObterTodos()).ToList();
            return Task.FromResult(resultado.Como<IList<TEntidade>>());
        }
        #endregion

        #region Implementação de suporte pro repositório
        protected IQueryable<TProxy> ObterTodos<T>(IFiltro<T> filtro)
        {
            return ObterTodos().Where(ConvertePredicado(filtro.ObterPredicados()));
        }
        protected IQueryable<TProxy> ObterTodos()
        {
            return context.ObterColecao<TProxy>().AsQueryable();
        }
        private Expression<Func<TProxy, bool>> ConvertePredicado<T>(Expression<Func<T, bool>> predicado)
        {
            return _mapper.Map<Expression<Func<TProxy, bool>>>(predicado);
        }
        private Expression<Func<IQueryable<TProxy>, IQueryable<TProxy>>> ConvertePredicado<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> predicado)
        {
            return _mapper.Map<Expression<Func<IQueryable<TProxy>, IQueryable<TProxy>>>>(predicado);
        }
        #endregion
    }
}
