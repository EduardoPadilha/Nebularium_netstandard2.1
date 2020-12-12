using Nebularium.Weaver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nebularium.Weaver
{
    public class GerenciadorAssinatura : IGerenciadorAssinatura
    {
        private readonly IDictionary<string, IList<IAssinatura>> _manipuladores = new Dictionary<string, IList<IAssinatura>>();

        public bool EstaVazio => !_manipuladores.Keys.Any();

        public event EventHandler<string> QuandoEventoRemovido;
        public event EventHandler<string> QuandoEventoAdicionado;


        public void AddAssinatura<TEvento, TManipuladoEvento>()
            where TEvento : IEvento
            where TManipuladoEvento : IManipuladorEvento<TEvento>
        {
            var manipuladorTipo = typeof(TManipuladoEvento);
            var eventoTipo = typeof(TEvento);
            if (!TemAssinaturaParaEvento<TEvento>())
            {
                QuandoEventoAdicionado?.Invoke(this, eventoTipo.Name);
                _manipuladores.Add(eventoTipo.Name, new List<IAssinatura>());
            }

            if (_manipuladores[eventoTipo.Name].Any(s => s.TipoManipulador == manipuladorTipo))
                throw new ArgumentException($"Manipulador do tipo {manipuladorTipo.Name} já registrado para '{eventoTipo.Name}'", manipuladorTipo.Name);

            _manipuladores[eventoTipo.Name].Add(Assinatura.New(eventoTipo, manipuladorTipo));
        }

        public void RemoverAssinatura<TEvento, TManipuladoEvento>()
            where TEvento : IEvento
            where TManipuladoEvento : IManipuladorEvento<TEvento>
        {

            var manipuladorParaRemover = ObterAssinaturaParaRemover<TEvento, TManipuladoEvento>();
            RemoverAssinatura<TEvento>(manipuladorParaRemover);
        }
        private IAssinatura ObterAssinaturaParaRemover<TEvento, TManipuladoEvento>()
            where TEvento : IEvento
            where TManipuladoEvento : IManipuladorEvento<TEvento>
        {
            if (!TemAssinaturaParaEvento<TEvento>())
                return null;

            return _manipuladores[typeof(TEvento).Name].SingleOrDefault(s => s.TipoManipulador == typeof(TManipuladoEvento));
        }
        private void RemoverAssinatura<TEvento>(IAssinatura assinaturaParaRemover) where TEvento : IEvento
        {
            var tipoEvento = typeof(TEvento);

            if (assinaturaParaRemover == null) return;

            _manipuladores[tipoEvento.Name].Remove(assinaturaParaRemover);

            if (_manipuladores[tipoEvento.Name].Any()) return;

            _manipuladores.Remove(tipoEvento.Name);
            QuandoEventoRemovido?.Invoke(this, tipoEvento.Name);
        }

        public bool TemAssinaturaParaEvento<TEvento>() => !EstaVazio && _manipuladores.ContainsKey(typeof(TEvento).Name);
        public bool TemAssinaturaParaEvento(string tipoEvento) => !EstaVazio && _manipuladores.ContainsKey(tipoEvento);
        public IEnumerable<IAssinatura> ObterManipuladores<TEvento>() => _manipuladores[typeof(TEvento).Name];
        public IEnumerable<IAssinatura> ObterManipuladores(string tipoEvento) => _manipuladores[tipoEvento];
    }
}
