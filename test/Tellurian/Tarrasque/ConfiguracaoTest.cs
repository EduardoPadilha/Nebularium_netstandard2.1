using Nebularium.Behemoth.Mongo.Configuracoes;
using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tellurian.Recursos;
using Xunit;
using Xunit.Abstractions;

namespace Nebularium.Tellurian.Tarrasque
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
            var dbConfig = gestorConfiguracao.ObterConfiguracao<DBConfiguracaoPadrao>();
            Assert.NotNull(dbConfig);
            Assert.Equal("mongodb://localhost:27017", dbConfig.StringConexao);
            Assert.Equal("ErpPdv", dbConfig.NomeBancoDados);
        }
    }
}
