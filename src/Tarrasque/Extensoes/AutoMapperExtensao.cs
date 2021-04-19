using AutoMapper;
using Nebularium.Tarrasque.Recursos;
using System;
using System.Linq.Expressions;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class AutoMapperExtensao
    {
        public static void CriarMapeamentoAutomaticoParaTipo<TInterface, TTConcreto, TAssembly>(this Profile profile)
        {
            profile.CriarMapeamentoAutomaticoParaTipo(typeof(TInterface), typeof(TTConcreto), typeof(TAssembly));
        }
        public static void CriarMapeamentoAutomaticoParaTipo<TInterface, TTConcreto>(this Profile profile, Type tipoNoAssemblyAlvo = null)
        {
            profile.CriarMapeamentoAutomaticoParaTipo(typeof(TInterface), typeof(TTConcreto), tipoNoAssemblyAlvo);
        }
        public static void CriarMapeamentoAutomaticoParaTipo<TInterface>(this Profile profile, Type tipoConcretoImplementacao, Type tipoNoAssemblyAlvo = null)
        {
            profile.CriarMapeamentoAutomaticoParaTipo(typeof(TInterface), tipoConcretoImplementacao, tipoNoAssemblyAlvo);
        }
        public static void CriarMapeamentoAutomaticoParaTipo(this Profile profile, Type interfaceEscaneada, Type tipoConcretoImplementacao, Type tipoNoAssemblyAlvo = null)
        {
            var dic = EscanerTipos.EscanearImplementacoesInterfacesNoAssembly(interfaceEscaneada, tipoNoAssemblyAlvo);
            foreach (var mapeamento in dic)
                profile.CreateMap(mapeamento.Key, tipoConcretoImplementacao);
        }

        public static void CriarMapeamentoAutomatico<TInterface>(this Profile profile, Type tipoNoAssemblyAlvo = null)
        {
            profile.CriarMapeamentoAutomatico(typeof(TInterface), tipoNoAssemblyAlvo);
        }
        public static void CriarMapeamentoAutomatico<TInterface, TAssembly>(this Profile profile)
        {
            profile.CriarMapeamentoAutomatico(typeof(TInterface), typeof(TAssembly));
        }
        public static void CriarMapeamentoAutomatico(this Profile profile, Type tipo, Type tipoNoAssemblyAlvo = null)
        {
            var dic = EscanerTipos.EscanearImplementacoesInterfacesNoAssembly(tipo, tipoNoAssemblyAlvo);
            foreach (var mapeamento in dic)
                profile.CreateMap(mapeamento.Key, mapeamento.Value);
        }

        public static IMappingExpression<TSource, TDestination> ForMemberMapFrom
            <TDestination, TDestinationMember, TSource, TSourceMember>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, TDestinationMember>> destinationMember,
            Expression<Func<TSource, TSourceMember>> mapExpression)
        {
            return map.ForMember(destinationMember, origem => origem.MapFrom(mapExpression));
        }
    }
}
