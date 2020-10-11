using AutoMapper;
using System.Collections.Generic;

namespace Nebularium.Tarrasque.Interfaces
{
    public interface IGestorMapeamento
    {
        IMapper Mapper { get; }
        IReadOnlyList<Profile> PerfisConfigurados { get; }
        void AdicionarPerfil<TPerfil>() where TPerfil : Profile, new();
    }
}
