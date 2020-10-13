using Nebularium.Tarrasque.Interfaces;

namespace Nebularium.Tarrasque.Configuracoes
{
    public class DBConfig : IDbConfigs
    {
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
