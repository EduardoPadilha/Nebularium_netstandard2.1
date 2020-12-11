using AutoMapper;
using Nebularium.Tellurian.Drone.Behemoth.Mapeamentos;
using Nebularium.Tellurian.Drone.Entidades;

namespace Nebularium.Tellurian.Recursos
{
    public class TellurianPerfilMapeamento : Profile
    {

        public TellurianPerfilMapeamento()
        {
            CreateMap<Pessoa, Pessoa>().ReverseMap();
            CreateMap<Pessoa, PessoaMapeamento>().ReverseMap();
        }
    }
}
