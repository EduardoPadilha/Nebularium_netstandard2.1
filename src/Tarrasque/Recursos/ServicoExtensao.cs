using Microsoft.Extensions.DependencyInjection;
using Nebularium.Tarrasque.Gestores;

namespace Nebularium.Tarrasque.Recursos
{
    public static class ServicoExtensao
    {
        /// <summary>
        /// Último servico a ser adicionado
        /// </summary>
        /// <param name="servicos"></param>
        /// <returns></returns>
        public static IServiceCollection AddGestorDependenciaAspnetPadrao(this IServiceCollection servicos)
        {
            AspnetGestorPadrao.Inicializar(servicos);
            return servicos;
        }
    }
}
