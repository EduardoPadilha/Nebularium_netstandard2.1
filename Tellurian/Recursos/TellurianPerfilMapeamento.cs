using AutoMapper;
using Nebularium.Tellurian.Mock;

namespace Nebularium.Tellurian.Recursos
{
    public class TellurianPerfilMapeamento : Profile
    {

        public TellurianPerfilMapeamento()
        {
            CreateMap<Pessoa, PessoaMProxy>().ReverseMap();
        }
    }
}
