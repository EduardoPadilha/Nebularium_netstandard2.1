using Nebularium.Behemoth.Mongo.Contextos;
using Nebularium.Behemoth.Mongo.Repositorios;
using Nebularium.Tellurian.Mock.Interfaces;

namespace Nebularium.Tellurian.Mock.Repositorios
{
    public class PessoaComandoRepositorio : ComandoRepositorio<Pessoa, PessoaMProxy>, IPessoaComandoRepositorio
    {
        public PessoaComandoRepositorio(IMongoContext context) : base(context)
        {
        }
    }
}
