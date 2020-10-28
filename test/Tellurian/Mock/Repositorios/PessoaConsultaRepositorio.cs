using Nebularium.Behemoth.Mongo.Contextos;
using Nebularium.Behemoth.Mongo.Repositorios;
using Nebularium.Tellurian.Mock;
using Nebularium.Tellurian.Mock.Interfaces;

namespace Nebularium.Tellurian.Behemoth.Repositorios
{
    public class PessoaConsultaRepositorio : ConsultaRepositorio<Pessoa, PessoaMProxy>, IPessoaConsultaRepositorio
    {
        public PessoaConsultaRepositorio(IMongoContext context) : base(context)
        {
        }
    }
}
