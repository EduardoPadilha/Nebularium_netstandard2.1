using AutoMapper;
using Nebularium.Behemoth.Mongo.Mapeamento;
using Nebularium.Tellurian.Drone.Behemoth.Mapeamentos;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tiamat.Entidades;

namespace Nebularium.Tellurian.Recursos
{
    public class TellurianPerfilMapeamento : Profile
    {

        public TellurianPerfilMapeamento()
        {
            CreateMap<Pessoa, Pessoa>().ReverseMap();
            CreateMap<Pessoa, PessoaMapeamento>().ReverseMap();
            CreateMap<Endereco, EnderecoMapeamento>().ReverseMap();
            CreateMap<Metadado, MetadadoMapeamento>().ReverseMap();
        }
    }
}
