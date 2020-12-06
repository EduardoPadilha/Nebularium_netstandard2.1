using Nebularium.Tarrasque.Configuracoes;
using Nebularium.Tarrasque.Recursos;
using Nebularium.Tellurian.Drone.Recursos;
using Xunit.Abstractions;

namespace Nebularium.Tellurian.Recursos
{
    public class TesteBase
    {
        protected readonly ITestOutputHelper Console;
        public TesteBase(ITestOutputHelper saida)
        {
            this.Console = saida;
            NebulariumFabrica.Inicializar();
            ConfiguracaoRecursos.AdicionarRecurso(typeof(Properties.Resources));
            Configuracao.DisplayNameExtrator = () => new DisplayNameExtratorPadrao();
        }
    }
}
