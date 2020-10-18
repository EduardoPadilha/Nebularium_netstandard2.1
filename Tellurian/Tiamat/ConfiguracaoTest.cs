using Nebularium.Tarrasque.Configuracoes;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tarrasque.Interfaces;
using Nebularium.Tellurian.Recursos;
using Xunit;
using Xunit.Abstractions;

namespace Nebularium.Tellurian.Tiamat.Configuracoes
{
    public class ConfiguracaoTest : TesteBase
    {
        private readonly IGestorConfiguracao gestorConfiguracao;
        public ConfiguracaoTest(ITestOutputHelper saida) : base(saida)
        {
            gestorConfiguracao = GestorDependencia.Instancia.ObterInstancia<IGestorConfiguracao>();
        }
        [Fact]
        public void GestorConfiguracao_test()
        {
            var dbConfig = gestorConfiguracao.ObterConfiguracao<DBConfig>();
            Assert.NotNull(dbConfig);
            Assert.Equal("mongodb://localhost:27017", dbConfig.ConnectionString);
            Assert.Equal("ErpPdv", dbConfig.DatabaseName);
        }
    }
}
