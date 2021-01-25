using AutoMapper;
using FluentValidation.Results;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Validacoes;

namespace Nebularium.Tiamat.Recursos
{
    public class PerfilMapeamento : Profile
    {
        public PerfilMapeamento()
        {
            CreateMap<ValidationResult, ValidacaoResultado>().ForMember(d => d.Erros, op => op.MapFrom(o => o.Errors));
            CreateMap<ValidationFailure, ErroValidacao>().ForMember(d => d.Mensagem, op => op.MapFrom(o => o.ErrorMessage))
                .ForMember(d => d.NomePropriedade, op => op.MapFrom(o => o.PropertyName));
            CreateMap<IPaginacao, Paginador>();
        }
    }
}
