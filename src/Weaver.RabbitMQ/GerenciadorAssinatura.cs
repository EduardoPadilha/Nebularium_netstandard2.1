using Nebularium.Weaver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nebularium.Weaver.RabbitMQ
{
    public class GerenciadorAssinatura : IGerenciadorAssinatura
    {
        private readonly IDictionary<string, IList<IAssinatura>> _manipuladores = new Dictionary<string, IList<IAssinatura>>();

        public bool EstaVazio => !_manipuladores.Keys.Any();

        public event EventHandler<string> QuandoEventoRemovido;
        public event EventHandler<string> QuandoEventoAdicionado;


        public void AddAssinatura<TEvento, TEventoManipulador>()
            where TEvento : IEvento
            where TEventoManipulador : IEventoManipulador<TEvento>
        {
            var manipuladorTipo = typeof(TEventoManipulador);
            var eventoTipo = typeof(TEvento);
            if (!TemAssinaturaParaEvento<TEvento>())
            {
                QuandoEventoAdicionado?.Invoke(this, eventoTipo.FullName);
                _manipuladores.Add(eventoTipo.FullName, new List<IAssinatura>());
            }

            if (_manipuladores[eventoTipo.FullName].Any(s => s.TipoManipulador == manipuladorTipo))
                throw new ArgumentException($"Manipulador do tipo {manipuladorTipo.Name} já registrado para '{eventoTipo.Name}'", manipuladorTipo.Name);

            _manipuladores[eventoTipo.FullName].Add(Assinatura.New(eventoTipo, manipuladorTipo));
        }

        public void RemoverAssinatura<TEvento, TEventoManipulador>()
            where TEvento : IEvento
            where TEventoManipulador : IEventoManipulador<TEvento>
        {

            var manipuladorParaRemover = ObterAssinaturaParaRemover<TEvento, TEventoManipulador>();
            RemoverAssinatura<TEvento>(manipuladorParaRemover);
        }
        private IAssinatura ObterAssinaturaParaRemover<TEvento, TEventoManipulador>()
            where TEvento : IEvento
            where TEventoManipulador : IEventoManipulador<TEvento>
        {
            if (!TemAssinaturaParaEvento<TEvento>())
                return null;

            return _manipuladores[typeof(TEvento).FullName].SingleOrDefault(s => s.TipoManipulador == typeof(TEventoManipulador));
        }
        private void RemoverAssinatura<TEvento>(IAssinatura assinaturaParaRemover) where TEvento : IEvento
        {
            var tipoEvento = typeof(TEvento);

            if (assinaturaParaRemover == null) return;

            _manipuladores[tipoEvento.FullName].Remove(assinaturaParaRemover);

            if (_manipuladores[tipoEvento.FullName].Any()) return;

            _manipuladores.Remove(tipoEvento.FullName);
            QuandoEventoRemovido?.Invoke(this, tipoEvento.FullName);
        }

        public bool TemAssinaturaParaEvento<TEvento>() => !EstaVazio && _manipuladores.ContainsKey(typeof(TEvento).FullName);
        public bool TemAssinaturaParaEvento(string tipoEvento) => !EstaVazio && _manipuladores.ContainsKey(tipoEvento);
        public IEnumerable<IAssinatura> ObterManipuladores<TEvento>() => _manipuladores[typeof(TEvento).FullName];
        public IEnumerable<IAssinatura> ObterManipuladores(string tipoEvento) => _manipuladores[tipoEvento];
    }
}
