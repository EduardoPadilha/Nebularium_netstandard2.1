using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Behemoth.Mongo.Extensoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nebularium.Behemoth.Mongo.Repositorios
{
    public abstract class RepositorioEntidadeBase<TEntidade, TProxy> : RepositorioBase<TProxy>, IRepositorioEntidadeBase<TEntidade>
        where TEntidade : IEntidade, new()
         where TProxy : IEntidade, new()
    {
        protected readonly ILogger<RepositorioEntidadeBase<TEntidade, TProxy>> logger;

        protected RepositorioEntidadeBase(IMongoContexto contexto, ILogger<RepositorioEntidadeBase<TEntidade, TProxy>> logger) : base(contexto)
        {
            this.logger = logger;
        }
        protected override string NomeColecao => typeof(TEntidade).Name.SnakeCase();

        #region Consultas
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
        #endregion

        #region Implementação de suporte para as consultas

        protected virtual async Task<IEnumerable<TEntidade>> ProcessarBuscas(IMongoQueryable<TProxy> query, IPaginador paginador)
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

        #region Comandos
        public virtual Task AdicionarAsync(TEntidade entidade)
        {
            try
            {
                var proxy = entidade.Como<TProxy>();
                return colecao.InsertOneAsync(proxy)
                     .ContinueWith(e => entidade.Injete(proxy));
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
                var proxys = entidades.Como<IEnumerable<TProxy>>();
                return colecao.InsertManyAsync(proxys)
                     .ContinueWith(e => entidades.Injete(proxys));
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
                var definicoes = propriedades.ObterUpdate<TProxy>();
                return colecao.UpdateOneAsync(predicado.ConvertePredicado<TEntidade, TProxy>(), definicoes).ContinueWith(task => task.Result.ModifiedCount >= 1);
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
                var definicoes = propriedades.ObterUpdate<TProxy>();
                return colecao.UpdateManyAsync(predicado.ConvertePredicado<TEntidade, TProxy>(), definicoes).ContinueWith(task => task.Result.ModifiedCount >= 1);
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
                return colecao.DeleteManyAsync(predicado.ConvertePredicado<TEntidade, TProxy>()).ContinueWith(task => task.Result.DeletedCount >= 1);
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
