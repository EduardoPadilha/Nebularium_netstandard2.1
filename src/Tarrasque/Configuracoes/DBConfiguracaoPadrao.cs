using Microsoft.Extensions.Configuration;
using Nebularium.Tarrasque.Abstracoes;

namespace Nebularium.Tarrasque.Configuracoes
{
    public class DBConfiguracaoPadrao : IDbConfiguracao
    {
        public DBConfiguracaoPadrao() { }
        public DBConfiguracaoPadrao(IConfiguration configuracao)
        {
            var section = configuracao.GetSection(Secao);
            ConnectionString = section["ConnectionString"];
            DatabaseName = section["DatabaseName"];
        }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public string Secao => "DBConfigDefault";
    }

    public interface IDbConfiguracao : IConfiguracao
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
