using MongoDB.Driver.Linq;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Behemoth.Mongo.Repositorios;
using Nebularium.Tellurian.Drone.Behemoth.Mapeamentos;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tellurian.Drone.Interfaces;

namespace Nebularium.Tellurian.Drone.Behemoth.Repositorios
{
    public class PessoaConsultaRepositorio : ConsultaRepositorio<Pessoa, PessoaMapeamento>, IPessoaConsultaRepositorio
    {
        public PessoaConsultaRepositorio(IMongoContexto context) : base(context)
        {
        }

        public override IOrderedMongoQueryable<PessoaMapeamento> OrdernarPadrao(IMongoQueryable<PessoaMapeamento> query)
        {
            return query.OrderBy(c => c.Nascimento);
        }
    }
}
