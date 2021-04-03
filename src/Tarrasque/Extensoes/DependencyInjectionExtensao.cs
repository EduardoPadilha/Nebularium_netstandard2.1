using Microsoft.Extensions.DependencyInjection;
using Nebularium.Tarrasque.Recursos;
using System;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class DependencyInjectionExtensao
    {
        public static IServiceCollection AddScopedTodosPorInterface(this IServiceCollection servicos, Type tipo, Type tipoNoAssemblyAlvo = null)
        {
            AddTodosServicosPorInterface(servicos.AddScoped, tipo, tipoNoAssemblyAlvo);
            return servicos;
        }
        public static IServiceCollection AddSingletonTodosPorInterface(this IServiceCollection servicos, Type tipo, Type tipoNoAssemblyAlvo = null)
        {
            AddTodosServicosPorInterface(servicos.AddSingleton, tipo, tipoNoAssemblyAlvo);
            return servicos;
        }
        public static IServiceCollection AddTransientTodosPorInterface(this IServiceCollection servicos, Type tipo, Type tipoNoAssemblyAlvo = null)
        {
            AddTodosServicosPorInterface(servicos.AddTransient, tipo, tipoNoAssemblyAlvo);
            return servicos;
        }

        private static void AddTodosServicosPorInterface(Func<Type, Type, IServiceCollection> addServico, Type tipo, Type tipoNoAssemblyAlvo = null)
        {
            var dic = EscanerTipos.EscanearImplementacoesInterfacesNoAssembly(tipo, tipoNoAssemblyAlvo);
            foreach (var mapeamento in dic)
                addServico.Invoke(mapeamento.Value, mapeamento.Key);
        }
    }
}
