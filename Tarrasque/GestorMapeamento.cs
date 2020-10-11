using AutoMapper;
using Nebularium.Tarrasque.Interfaces;
using System.Collections.Generic;

namespace Nebularium.Tarrasque
{
    public abstract class GestorMapeamento<T> : IGestorMapeamento where T : IGestorMapeamento, new()
    {
        public static IGestorMapeamento Instancia { get; protected set; }
        public static void Inicializar()
        {
            if (Instancia != null) return;

            Instancia = new T();
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var p in Instancia.PerfisConfigurados)
                    cfg.AddProfile(p);
            });
            ((GestorMapeamento<T>)Instancia).Mapper = config.CreateMapper();
        }

        public GestorMapeamento()
        {
        }

        public IMapper Mapper { get; protected set; }

        public IReadOnlyList<Profile> PerfisConfigurados => _perfisConfigurados;
        private List<Profile> _perfisConfigurados;

        public void AdicionarPerfil<TPerfil>() where TPerfil : Profile, new()
        {
            if (_perfisConfigurados == null)
                _perfisConfigurados = new List<Profile>();
            var perfil = new TPerfil();
            _perfisConfigurados.Add(perfil);
        }

    }
}
