using AutoMapper;
using Nebularium.Tarrasque.Abstracoes;
using System;
using System.Collections.Generic;

namespace Nebularium.Tarrasque.Gestores
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
                if (Instancia.ConfigsAdicionais != null)
                    foreach (var exp in Instancia.ConfigsAdicionais)
                        exp.Invoke(cfg);

                foreach (var p in Instancia.PerfisConfigurados)
                    cfg.AddProfile(p);
            });
            ((GestorMapeamento<T>)Instancia).Mapper = config.CreateMapper();
        }

        public GestorMapeamento()
        {
        }

        public IMapper Mapper { get; protected set; }

        public IReadOnlyList<Action<IMapperConfigurationExpression>> ConfigsAdicionais => _configsAdicionais;
        private List<Action<IMapperConfigurationExpression>> _configsAdicionais;
        public void AdicionarConfig(Action<IMapperConfigurationExpression> config)
        {
            if (_configsAdicionais == null)
                _configsAdicionais = new List<Action<IMapperConfigurationExpression>>();
            _configsAdicionais.Add(config);
        }

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
