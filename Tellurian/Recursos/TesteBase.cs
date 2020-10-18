using Nebularium.Tarrasque.Configuracoes;
using Nebularium.Tarrasque.Recursos;
using Xunit.Abstractions;

namespace Nebularium.Tellurian.Recursos
{
    public class TesteBase
    {
        protected readonly ITestOutputHelper saida;
        public TesteBase(ITestOutputHelper saida)
        {
            this.saida = saida;
            NebulariumFabrica.Inicializar();
            ConfiguracaoRecursos.AdicionarRecurso(typeof(Properties.Resources));
            Configuracao.DisplayNameExtrator = () => new DisplayNameExtratorPadrao();
        }
    }
}
