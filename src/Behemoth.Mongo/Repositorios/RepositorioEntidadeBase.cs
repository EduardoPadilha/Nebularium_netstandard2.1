using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Behemoth.Mongo.Extensoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Recursos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nebularium.Behemoth.Mongo.Repositorios
{
    public abstract class RepositorioEntidadeBase<TEntidade> : RepositorioBase<TEntidade>, IRepositorioEntidadeBase<TEntidade>
        where TEntidade : IEntidade, new()
    {
        protected readonly ILogger<RepositorioEntidadeBase<TEntidade>> logger;

        protected RepositorioEntidadeBase(IMongoContexto contexto, ILogger<RepositorioEntidadeBase<TEntidade>> logger) : base(contexto)
        {
            this.logger = logger;
        }

        #region Consultas
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

        #region Implementação de suporte para as consultas

        protected virtual IFindFluent<TEntidade, TEntidade> Paginar(IFindFluent<TEntidade, TEntidade> query, IPaginador paginador)
        {
            if (paginador.TotalRegistros == 0)
                return default;
            return query.Skip((paginador.Pagina - 1) * paginador.TamanhoPagina).Limit(paginador.TamanhoPagina);
        }

        protected virtual async Task<IEnumerable<TEntidade>> ProcessarBuscas(IFindFluent<TEntidade, TEntidade> query, IPaginador paginador)
        {
            if (paginador == null)
                return await OrdenacaoPadrao(query).ToListAsync();

            var total = await query.CountDocumentsAsync();

            paginador.IniciaPaginador(total);
            var queryOrdenada = OrdenacaoPadrao(query);
            var queryPaginada = Paginar(queryOrdenada, paginador);
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

        #region Comandos
        public virtual Task AdicionarAsync(TEntidade entidade)
        {
            try
            {
                return colecao.InsertOneAsync(entidade)
                     .ContinueWith(e => entidade.Injete(entidade));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao adicionar entidade");
                throw;
            }
        }

        public virtual Task AdicionarAsync(IEnumerable<TEntidade> entidades)
        {
            try
            {
                return colecao.InsertManyAsync(entidades)
                     .ContinueWith(e => entidades.Injete(entidades));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao adicionar entidade");
                throw;
            }
        }

        public virtual Task<bool> AtualizarUmAsync(Expression<Func<TEntidade, bool>> predicado, List<PropriedadeValor> propriedades)
        {
            try
            {
                var definicoes = propriedades.ObterUpdate<TEntidade>();
                return colecao.UpdateOneAsync(predicado, definicoes).ContinueWith(task => task.Result.ModifiedCount >= 1);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao atualizar a entidade");
                throw;
            }
        }

        public virtual Task<bool> AtualizarMuitosAsync(Expression<Func<TEntidade, bool>> predicado, List<PropriedadeValor> propriedades)
        {
            try
            {
                var definicoes = propriedades.ObterUpdate<TEntidade>();
                return colecao.UpdateManyAsync(predicado, definicoes).ContinueWith(task => task.Result.ModifiedCount >= 1);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao atualizar a entidade");
                throw;
            }
        }

        public virtual Task<bool> RemoverUmAsync(string id)
        {
            try
            {
                return colecao.DeleteOneAsync(x => x.Id == id).ContinueWith(task => task.Result.DeletedCount >= 1);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao renover entidade");
                throw;
            }
        }

        public virtual Task<bool> RemoverMuitosAsync(Expression<Func<TEntidade, bool>> predicado)
        {
            try
            {
                return colecao.DeleteManyAsync(predicado).ContinueWith(task => task.Result.DeletedCount >= 1);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao remover entidade");
                throw;
            }
        }
        #endregion
    }
}
