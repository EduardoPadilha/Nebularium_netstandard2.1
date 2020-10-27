using Microsoft.Extensions.Configuration;
using Nebularium.Tarrasque.Interfaces;

namespace Nebularium.Tarrasque.Configuracoes
{
    public class DBConfig : IDbConfigs
    {
        public DBConfig() { }
        public DBConfig(IConfiguration configuracao)
        {
            var section = configuracao.GetSection(Secao);
            ConnectionString = section["ConnectionString"];
            DatabaseName = section["DatabaseName"];
        }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public string Secao => "DBConfigDefault";
    }

    public interface IDbConfigs : IConfiguracao
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
