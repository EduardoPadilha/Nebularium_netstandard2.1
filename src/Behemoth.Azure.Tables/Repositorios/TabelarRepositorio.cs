using Microsoft.Azure.Cosmos.Table;
using Nebularium.Behemoth.Azure.Tables.Abstracoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nebularium.Behemoth.Azure.Tables.Repositorios
{
    public abstract class TabelarRepositorio<TEntidade, TProxy> :
        TabelarRepositorioBase<TProxy>,
        ITabelarRepositorio<TEntidade>
        where TProxy : ITableEntity, new()
    {
        protected TabelarRepositorio(IAzureTableContexto contexto) : base(contexto)
        {
        }

        public virtual async Task AdicionarAsync(TEntidade entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException("entidade");

            var proxy = entidade.Como<TProxy>();
            try
            {
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(proxy);
                TableResult result = await tabela.ExecuteAsync(insertOrMergeOperation);
                var resultado = result.Result;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        public virtual async Task<bool> AtualizarAsync(TEntidade entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException("entidade");

            var proxy = entidade.Como<TProxy>();
            proxy.ETag = "*";
            try
            {
                TableOperation insertOrMergeOperation = TableOperation.Merge(proxy);
                TableResult result = await tabela.ExecuteAsync(insertOrMergeOperation);
                var resultado = result.Result;
                return resultado != null;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        public virtual async Task<bool> RemoverAsync(TEntidade entidade)
        {
            var proxy = entidade.Como<TProxy>();
            proxy.ETag = "*";
            TableOperation insertOrMergeOperation = TableOperation.Delete(proxy);
            TableResult result = await tabela.ExecuteAsync(insertOrMergeOperation);
            var resultado = result.Result;
            return resultado != null;
        }

        protected virtual TProxy Obter(string chaveParticao, string chaveLinha)
        {
            return tabela.CreateQuery<TProxy>()
                 .Where(x => x.PartitionKey == chaveParticao && x.RowKey == chaveLinha)
                 .SingleOrDefault();
        }

        public virtual Task<TEntidade> ObterAsync(string chaveParticao, string chaveLinha)
        {
            var retorno = Obter(chaveParticao, chaveLinha);

            var convertido = retorno.Como<TEntidade>();
            return Task.FromResult(convertido);
        }

        public virtual Task<IEnumerable<TEntidade>> ObterTodosAsync(string chaveParticao, IPaginador paginador = null)
        {
            var query = tabela.CreateQuery<TProxy>().Where(x => x.PartitionKey == chaveParticao).ToList();
            var processados = ProcessarBuscas(query, paginador);
            return Task.FromResult(processados);
        }

        protected virtual IEnumerable<TProxy> Paginar(IEnumerable<TProxy> query, IPaginador paginador)
        {
            if (paginador.TotalRegistros == 0)
                return default;
            return query.Skip((paginador.Pagina - 1) * paginador.TamanhoPagina).Take(paginador.TamanhoPagina);
        }

        public virtual IOrderedEnumerable<TProxy> OrdenacaoPadrao(IEnumerable<TProxy> query)
        {
            return query.OrderBy(c => c.RowKey);
        }

        protected virtual IEnumerable<TEntidade> ProcessarBuscas(IEnumerable<TProxy> lista, IPaginador paginador)
        {
            if (paginador == null)
                return OrdenacaoPadrao(lista).Como<List<TEntidade>>();

            var total = lista.Count();

            paginador.IniciaPaginador(total);
            var queryOrdenada = OrdenacaoPadrao(lista);
            var queryPaginada = Paginar(queryOrdenada, paginador);
            if (queryPaginada == default)
                return default;

            return queryPaginada.Como<List<TEntidade>>();
        }
    }
}
