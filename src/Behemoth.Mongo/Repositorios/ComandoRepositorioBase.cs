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
    public abstract class ComandoRepositorioBase<TEntidade, TProxy> : IComandoRepositorioBase<TEntidade>
        where TEntidade : IEntidade, new()
        where TProxy : IEntidade, new()
    {
        protected readonly IMongoContexto contexto;
        protected readonly IMongoCollection<TProxy> colecao;
        private readonly ILogger logger;
        public ComandoRepositorioBase(IMongoContexto contexto, ILogger<TEntidade> logger)
        {
            this.contexto = contexto;
            this.logger = logger;
            colecao = contexto.ObterColecao<TProxy>();
        }

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
    }
}
