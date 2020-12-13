using Nebularium.Tarrasque.Gestores;
using Nebularium.Weaver.Exceccoes;
using Nebularium.Weaver.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Nebularium.Weaver
{
    public class Assinatura : IAssinatura
    {
        public Type TipoManipulador { get; }
        public Type TipoEvento { get; }

        private Assinatura(Type tipoEvento, Type tipoManipulador)
        {
            TipoManipulador = tipoManipulador;
            TipoEvento = tipoEvento;
        }

        public virtual async Task Resolver(string mensagem)
        {
            var dadosEvento = JsonConvert.DeserializeObject(mensagem, TipoEvento);
            var manipulador = GestorDependencia.Instancia.ObterInstancia(TipoManipulador);
            if (manipulador == null)
                throw new SemManipuladorException(TipoManipulador);
            var tipoConcreto = typeof(IManipuladorEvento<>).MakeGenericType(TipoEvento);
            var nomeMetodo = nameof(IManipuladorEvento<IEvento>.Resolver);
            await (Task)tipoConcreto.GetMethod(nomeMetodo).Invoke(manipulador, new[] { dadosEvento });
        }

        public static Assinatura New(Type tipoEvento, Type tipoManipulador) =>
            new Assinatura(tipoEvento, tipoManipulador);

        public static Assinatura New<TEvento, TEventoManipulador>() =>
            new Assinatura(typeof(TEvento), typeof(TEventoManipulador));
    }
}
