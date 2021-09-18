using MongoDB.Driver.Linq;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Behemoth.Mongo.Repositorios;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tellurian.Drone.Behemoth.Mapeamentos;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tellurian.Drone.Interfaces.Repositorios;

namespace Nebularium.Tellurian.Drone.Behemoth.Repositorios
{
    public class PessoaConsultaRepositorio : ConsultaRepositorioBase<Pessoa, PessoaMapeamento>, IPessoaConsultaRepositorio
    {
        public PessoaConsultaRepositorio(IMongoContexto context) : base(context)
        {
        }

        public override IOrderedMongoQueryable<PessoaMapeamento> OrdernarPadrao(IMongoQueryable<PessoaMapeamento> query)
        {
            return query.OrderBy(c => c.Nascimento);
        }

        protected override string NomeColecao => typeof(Pessoa).Name.SnakeCase();
    }
}
