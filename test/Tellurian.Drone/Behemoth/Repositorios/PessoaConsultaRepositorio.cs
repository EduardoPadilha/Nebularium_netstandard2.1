using Nebularium.Behemoth.Mongo.Contextos;
using Nebularium.Behemoth.Mongo.Repositorios;
using Nebularium.Tellurian.Drone.Behemoth.Mapeamentos;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tellurian.Drone.Interfaces;

namespace Nebularium.Tellurian.Drone.Behemoth.Repositorios
{
    public class PessoaConsultaRepositorio : ConsultaRepositorio<Pessoa, PessoaMapeamento>, IPessoaConsultaRepositorio
    {
        public PessoaConsultaRepositorio(IMongoContext context) : base(context)
        {
        }
    }
}
