using Nebularium.Weaver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nebularium.Weaver
{
    public class GerenciadorAssinatura
    {
        private readonly IDictionary<Type, IList<Assinatura>> _manipuladores = new Dictionary<Type, IList<Assinatura>>();

        public bool EstaVazio => !_manipuladores.Keys.Any();

        public event EventHandler<Type> QuandoEventoRemovido;
        public event EventHandler<Type> QuandoEventoAdicionado;


        public void AddAssinatura<TEvento, TEventoManipulador>()
            where TEvento : IEvento
            where TEventoManipulador : IEventoManipulador<TEvento>
        {
            var manipuladorTipo = typeof(TEventoManipulador);
            var eventoTipo = typeof(TEvento);
            if (!TemAssinaturaParaEvento<TEvento>())
            {
                _manipuladores.Add(eventoTipo, new List<Assinatura>());
                QuandoEventoAdicionado?.Invoke(this, eventoTipo);
            }

            if (_manipuladores[eventoTipo].Any(s => s.TipoManipulador == manipuladorTipo))
                throw new ArgumentException($"Manipulador do tipo {manipuladorTipo.Name} já registrado para '{eventoTipo.Name}'", manipuladorTipo.Name);

            _manipuladores[eventoTipo].Add(Assinatura.New(manipuladorTipo, eventoTipo));
        }

        public void RemoverAssinatura<TEvento, TEventoManipulador>()
            where TEvento : IEvento
            where TEventoManipulador : IEventoManipulador<TEvento>
        {

            var manipuladorParaRemover = ObterAssinaturaParaRemover<TEvento, TEventoManipulador>();
            RemoverAssinatura<TEvento>(manipuladorParaRemover);
        }
        private Assinatura ObterAssinaturaParaRemover<TEvento, TEventoManipulador>()
            where TEvento : IEvento
            where TEventoManipulador : IEventoManipulador<TEvento>
        {
            if (!TemAssinaturaParaEvento<TEvento>())
                return null;

            return _manipuladores[typeof(TEvento)].SingleOrDefault(s => s.TipoManipulador == typeof(TEventoManipulador));
        }
        private void RemoverAssinatura<TEvento>(Assinatura assinaturaParaRemover) where TEvento : IEvento
        {
            var tipoEvento = typeof(TEvento);

            if (assinaturaParaRemover == null) return;

            _manipuladores[tipoEvento].Remove(assinaturaParaRemover);

            if (_manipuladores[tipoEvento].Any()) return;

            _manipuladores.Remove(tipoEvento);
            QuandoEventoRemovido?.Invoke(this, tipoEvento);
        }

        public bool TemAssinaturaParaEvento<TEvento>() => _manipuladores.ContainsKey(typeof(TEvento));
        public bool TemAssinaturaParaEvento(Type tipoEvento) => _manipuladores.ContainsKey(tipoEvento);
        public IEnumerable<Assinatura> ObterManipuladores<TEvento>() => _manipuladores[typeof(TEvento)];
        public IEnumerable<Assinatura> ObterManipuladores(Type tipoEvento) => _manipuladores[tipoEvento];
    }
}
