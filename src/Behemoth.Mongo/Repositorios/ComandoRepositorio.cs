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
        public ComandoRepositorio(IMongoContext context)
        {
            this.context = context;
        }

        public Task AdicionarAsync(TEntidade entidade)
        {
            var proxy = entidade.Como<TProxy>();
            return context.ObterColecao<TProxy>().InsertOneAsync(proxy)
                 .ContinueWith(e => entidade.Injete(proxy));
        }

        public Task AdicionarAsync(IEnumerable<TEntidade> entidades)
        {
            var proxys = entidades.Como<IEnumerable<TProxy>>();
            return context.ObterColecao<TProxy>().InsertManyAsync(proxys)
                 .ContinueWith(e => entidades.Injete(proxys));
        }

        public Task AtualizarAsync(TEntidade entidade)
        {
            var proxy = entidade.Como<TProxy>();
            return context.ObterColecao<TProxy>().ReplaceOneAsync(x => x.Id == entidade.Id, proxy);
        }

        //public Task AtualizarAsync(IEnumerable<TEntidade> entidades)
        //{
        //    var proxys = entidades.Como<IEnumerable<TProxy>>();
        //    return context.ObterColecao<TProxy>().UpdateManyAsync(proxys)
        //         .ContinueWith(e => entidades.Injete(proxys));
        //}

        public Task RemoverAsync(TEntidade entidade)
        {
            return context.ObterColecao<TProxy>().DeleteOneAsync(x => x.Id == entidade.Id);
        }

        public Task RemoverAsync(IEnumerable<TEntidade> entidades)
        {
            var proxyIds = entidades.Select(e => e.Id);
            return context.ObterColecao<TProxy>().DeleteManyAsync(x => proxyIds.Contains(x.Id));
        }
    }
}
