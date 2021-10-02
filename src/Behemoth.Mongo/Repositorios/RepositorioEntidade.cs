using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Entidades;
using Nebularium.Tiamat.Recursos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nebularium.Behemoth.Mongo.Repositorios
{
    public abstract class RepositorioEntidade<TEntidade> : RepositorioEntidadeBase<TEntidade>,
       IRepositorioEntidade<TEntidade>
       where TEntidade : Entidade, new()
    {
        protected RepositorioEntidade(IMongoContexto contexto, ILogger<RepositorioEntidadeBase<TEntidade>> logger) : base(contexto, logger)
        {
        }

        public async override Task AdicionarAsync(IEnumerable<TEntidade> entidades)
        {
            foreach (var entidade in entidades)
            {
                await ValidaUnicidade(entidade);
                entidade.Metadado.Criar();
            }

            await base.AdicionarAsync(entidades);
        }

        public async override Task AdicionarAsync(TEntidade entidade)
        {
            await ValidaUnicidade(entidade);

            entidade.Metadado.Criar();

            await base.AdicionarAsync(entidade);
        }

        public async override Task<bool> AtualizarMuitosAsync(Expression<Func<TEntidade, bool>> predicado, List<PropriedadeValor> propriedades)
        {
            await ValidaUnicidadeAtualizacao(predicado, propriedades);

            propriedades.Add(PropriedadeValor.Cria<TEntidade, DateTimeOffset?>(c => c.Metadado.DataAtualizacao, DateTimeOffset.UtcNow));

            return await base.AtualizarMuitosAsync(predicado, propriedades);
        }

        public async override Task<bool> AtualizarUmAsync(Expression<Func<TEntidade, bool>> predicado, List<PropriedadeValor> propriedades)
        {
            await ValidaUnicidadeAtualizacao(predicado, propriedades);

            propriedades.Add(PropriedadeValor.Cria<TEntidade, DateTimeOffset?>(c => c.Metadado.DataAtualizacao, DateTimeOffset.UtcNow));
            return await base.AtualizarUmAsync(predicado, propriedades);
        }

        public override Task<bool> RemoverMuitosAsync(Expression<Func<TEntidade, bool>> predicado)
        {
            var propriedades = PropriedadeValorFabrica<TEntidade>.Iniciar()
                .Add(c => c.Metadado.DataDelecao, DateTimeOffset.UtcNow)
                .Add(c => c.Metadado.Ativo, false);
            return base.AtualizarMuitosAsync(predicado, propriedades.ObterTodos);
        }

        public override Task<bool> RemoverUmAsync(string id)
        {
            var propriedades = PropriedadeValorFabrica<TEntidade>.Iniciar()
                .Add(c => c.Metadado.DataDelecao, DateTimeOffset.UtcNow)
                .Add(c => c.Metadado.Ativo, false, false);
            return base.AtualizarUmAsync(c => c.Id == id, propriedades.ObterTodos);
        }


        public async virtual Task<bool> AtivarDesativarUmAsync(string id, bool ativar)
        {
            if (ativar)
                await ValidaUnicidadeAtivacao(id);

            var propriedades = PropriedadeValorFabrica<TEntidade>.Iniciar()
                .Add(c => c.Metadado.Ativo, ativar, false);
            return await AtualizarUmAsync(c => c.Id == id, propriedades.ObterTodos);
        }
        private async Task ValidaUnicidadeAtivacao(string id)
        {
            var entidade = await colecao.FindAsync(c => c.Id == id);
            await ValidaUnicidade(entidade.Como<TEntidade>());
        }

        protected abstract Task ValidaUnicidade(TEntidade entidade);
        protected abstract Task ValidaUnicidadeAtualizacao(Expression<Func<TEntidade, bool>> predicado, List<PropriedadeValor> propriedades);
    }
}
