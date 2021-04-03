using Microsoft.Extensions.Logging;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tellurian.Drone.Interfaces.Comandos;
using Nebularium.Tiamat.Abstracoes;
using Noctua.Dominio.Features;

namespace Nebularium.Tellurian.Drone.Comandos
{
    public class PessoaComandos : ComandosCrud<Pessoa>, IPessoaComandos
    {
        public PessoaComandos(IContextoNotificacao notificacao,
            IComandoRepositorio<Pessoa> repositorio,
            ILogger<Pessoa> logger) : base(notificacao, repositorio, logger)
        {

        }
    }
}
