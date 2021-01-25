using Microsoft.Extensions.Configuration;
using Nebularium.Tarrasque.Abstracoes;

namespace Nebularium.Behemoth.Mongo.Configuracoes
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

        public virtual string Secao => "DBConfigDefault";
    }
}
