using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Nebularium.Behemoth.Mongo.Contextos;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nebularium.Behemoth.Mongo.Repositorios
{
    public class ComandoRepositorio<TEntidade, TProxy> : IComandoRepositorio<TEntidade>
        where TEntidade : IEntidade, new()
        where TProxy : IEntidade, new()
    {
        protected IMongoContext context { get; }
        private readonly ILogger logger;
        public ComandoRepositorio(IMongoContext context, ILogger<TEntidade> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public Task AdicionarAsync(TEntidade entidade)
        {
            try
            {
                var proxy = entidade.Como<TProxy>();
                return context.ObterColecao<TProxy>().InsertOneAsync(proxy)
                     .ContinueWith(e => entidade.Injete(proxy));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao adicionar entidade");
                throw;
            }
        }

        public Task AdicionarAsync(IEnumerable<TEntidade> entidades)
        {
            try
            {
                var proxys = entidades.Como<IEnumerable<TProxy>>();
                return context.ObterColecao<TProxy>().InsertManyAsync(proxys)
                     .ContinueWith(e => entidades.Injete(proxys));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao adicionar entidade");
                throw;
            }
        }

        public Task AtualizarAsync(TEntidade entidade)
        {
            try
            {
                var proxy = entidade.Como<TProxy>();
                return context.ObterColecao<TProxy>().ReplaceOneAsync(x => x.Id == entidade.Id, proxy);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao adicionar entidade");
                throw;
            }
        }

        //public Task AtualizarAsync(IEnumerable<TEntidade> entidades)
        //{
        //    var proxys = entidades.Como<IEnumerable<TProxy>>();
        //    return context.ObterColecao<TProxy>().UpdateManyAsync(proxys)
        //         .ContinueWith(e => entidades.Injete(proxys));
        //}

        public Task RemoverAsync(TEntidade entidade)
        {
            try
            {
                return context.ObterColecao<TProxy>().DeleteOneAsync(x => x.Id == entidade.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao adicionar entidade");
                throw;
            }
        }

        public Task RemoverAsync(IEnumerable<TEntidade> entidades)
        {
            try
            {
                var proxyIds = entidades.Select(e => e.Id);
                return context.ObterColecao<TProxy>().DeleteManyAsync(x => proxyIds.Contains(x.Id));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao adicionar entidade");
                throw;
            }
        }
    }
}
