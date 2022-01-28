using Microsoft.Azure.Cosmos.Table;
using Nebularium.Behemoth.Abstracoes;
using Nebularium.Behemoth.Atributos;
using Nebularium.Behemoth.Azure.Tables.Abstracoes;
using Nebularium.Tarrasque.Extensoes;
using System;
using System.Threading.Tasks;

namespace Nebularium.Behemoth.Azure.Tables
{
    public abstract class AzureTableContextoBase : IAzureTableContexto
    {
        private readonly CloudStorageAccount contaArmazenamento;
        protected IDbConfiguracao configuracao;
        protected CloudTableClient cliente;
        public IDbConfiguracao ObterConfiguracao => configuracao;
        public CloudTableClient ObterCliente => cliente;

        public AzureTableContextoBase(IDbConfiguracao configuracao)
        {
            this.configuracao = configuracao;
            contaArmazenamento = CriarContaArmazenamentoDaStringConexao(configuracao.StringConexao);
            cliente = contaArmazenamento.CreateCloudTableClient(ObterConfiguracaoCliente());
        }

        protected virtual TableClientConfiguration ObterConfiguracaoCliente()
        {
            return new TableClientConfiguration();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task<CloudTable> ObterTabela<T>()
        {
            var nome = typeof(T).ObterAnotacao<NomeAttribute>()?.Nome;
            return ObterTabela<T>(nome.LimpoNuloBranco() ? typeof(T).Name.SnakeCase() : nome);
        }

        public Task<CloudTable> ObterTabela<T>(string nomeTabela)
        {
            return CriaTabelaSeNaoExisteAsync(nomeTabela, cliente);
        }

        #region Suporte
        public static CloudStorageAccount CriarContaArmazenamentoDaStringConexao(string stringConexeao)
        {
            CloudStorageAccount contaArmazenamento;
            try
            {
                contaArmazenamento = CloudStorageAccount.Parse(stringConexeao);
            }
            catch (FormatException)
            {
                Console.WriteLine("Informações de conta de armazenamento inválidas. Confirme se AccountName e AccountKey são válidas no app.config - então restart a aplicação.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Informações de conta de armazenamento inválidas. Confirme se AccountName e AccountKey são válidas no app.config - então restart a aplicação.");
                Console.ReadLine();
                throw;
            }

            return contaArmazenamento;
        }

        public static async Task<CloudTable> CriaTabelaSeNaoExisteAsync(string nomeTabela, CloudTableClient cliente)
        {
            CloudTable table = cliente.GetTableReference(nomeTabela);
            if (await table.CreateIfNotExistsAsync())
                Console.WriteLine("Criando tabela {0}", nomeTabela);
            else
                Console.WriteLine("Tabela {0} já existe", nomeTabela);

            Console.WriteLine();
            return table;
        }
        #endregion
    }
}
