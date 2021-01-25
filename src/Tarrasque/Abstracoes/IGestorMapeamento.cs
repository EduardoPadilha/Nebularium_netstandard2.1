using AutoMapper;
using System;
using System.Collections.Generic;

namespace Nebularium.Tarrasque.Abstracoes
{
    public interface IGestorMapeamento
    {
        IMapper Mapper { get; }
        IReadOnlyList<Profile> PerfisConfigurados { get; }
        IReadOnlyList<Action<IMapperConfigurationExpression>> ConfigsAdicionais { get; }
        void AdicionarPerfil<TPerfil>() where TPerfil : Profile, new();
        void AdicionarConfig(Action<IMapperConfigurationExpression> config);
    }
}
