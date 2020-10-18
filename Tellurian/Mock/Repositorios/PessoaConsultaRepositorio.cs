using AutoMapper;
using Nebularium.Behemoth.Mongo;
using Nebularium.Tellurian.Mock;
using Nebularium.Tellurian.Mock.Interfaces;

namespace Nebularium.Tellurian.Behemoth.Repositorios
{
    public class PessoaConsultaRepositorio : ConsultaRepositorio<Pessoa, PessoaMProxy>, IPessoaConsultaRepositorio
    {
        public PessoaConsultaRepositorio(IMongoContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
