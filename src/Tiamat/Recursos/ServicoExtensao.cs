using Microsoft.Extensions.DependencyInjection;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Notificacao;

namespace Nebularium.Tiamat.Recursos
{
    public static class ServicoExtensao
    {
        /// <summary>
        /// Último servico a ser adicionado
        /// </summary>
        /// <param name="servicos"></param>
        /// <returns></returns>
        public static IServiceCollection AddContextoNotificacao(this IServiceCollection servicos)
        {
            servicos.AddScoped<IContextoNotificacao, ContextoNotificacao>();
            return servicos;
        }
    }
}
