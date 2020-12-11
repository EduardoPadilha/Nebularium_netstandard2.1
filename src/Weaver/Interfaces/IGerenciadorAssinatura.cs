using System;
using System.Collections.Generic;

namespace Nebularium.Weaver.Interfaces
{
    public interface IGerenciadorAssinatura
    {
        bool EstaVazio { get; }

        event EventHandler<string> QuandoEventoAdicionado;
        event EventHandler<string> QuandoEventoRemovido;

        void AddAssinatura<TEvento, TEventoManipulador>()
            where TEvento : IEvento
            where TEventoManipulador : IEventoManipulador<TEvento>;
        IEnumerable<IAssinatura> ObterManipuladores(string tipoEvento);
        IEnumerable<IAssinatura> ObterManipuladores<TEvento>();
        void RemoverAssinatura<TEvento, TEventoManipulador>()
            where TEvento : IEvento
            where TEventoManipulador : IEventoManipulador<TEvento>;
        bool TemAssinaturaParaEvento(string tipoEvento);
        bool TemAssinaturaParaEvento<TEvento>();
    }
}