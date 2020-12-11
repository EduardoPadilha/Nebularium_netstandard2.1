using Microsoft.Extensions.Logging;
using Nebularium.Behemoth.Mongo.Contextos;
using Nebularium.Behemoth.Mongo.Repositorios;
using Nebularium.Tellurian.Drone.Behemoth.Mapeamentos;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tellurian.Drone.Interfaces;

namespace Nebularium.Tellurian.Drone.Behemoth.Repositorios
{
    public class PessoaComandoRepositorio : ComandoRepositorio<Pessoa, PessoaMapeamento>, IPessoaComandoRepositorio
    {
        public PessoaComandoRepositorio(IMongoContext context, ILogger<Pessoa> logger) : base(context, logger)
        {
        }
    }
}
