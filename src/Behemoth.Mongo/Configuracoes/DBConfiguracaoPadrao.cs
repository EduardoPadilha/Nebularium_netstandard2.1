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
            StringConexao = section["ConnectionString"];
            NomeBancoDados = section["DatabaseName"];
            LogLigado = section.GetValue<bool>("LogLigado");
        }
        public string StringConexao { get; set; }
        public string NomeBancoDados { get; set; }
        public bool LogLigado { get; set; }

        public virtual string Secao => "DBConfigDefault";
    }
}
