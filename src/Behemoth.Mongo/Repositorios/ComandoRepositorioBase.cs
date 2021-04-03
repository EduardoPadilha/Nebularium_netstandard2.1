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
    public abstract class ComandoRepositorioBase<TEntidade> : RepositorioBase<TEntidade>, IComandoRepositorioBase<TEntidade>
        where TEntidade : IEntidade, new()
    {
        protected readonly ILogger<TEntidade> logger;
        public ComandoRepositorioBase(IMongoContexto contexto, ILogger<TEntidade> logger) : base(contexto)
        {
            this.logger = logger;
        }

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
    }
}
