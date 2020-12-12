using Microsoft.Extensions.Logging;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tellurian.Drone.Eventos;
using Nebularium.Tiamat.Interfaces;
using Nebularium.Weaver.Interfaces;
using System.Threading.Tasks;

namespace Nebularium.Tellurian.Drone.Manipuladores
{
    public class CadastrarPessoaManipulador : IManipuladorEvento<CadastrarPessoaComandoEvento>
    {
        private readonly IComandoServico<Pessoa> pessoaComandoServico;
        private readonly ILogger<Pessoa> logger;
        public CadastrarPessoaManipulador(IComandoServico<Pessoa> pessoaComandoServico, ILogger<Pessoa> logger)
        {
            this.pessoaComandoServico = pessoaComandoServico;
            this.logger = logger;
        }


        public Task Resolver(CadastrarPessoaComandoEvento evento)
        {
            logger.LogInformation($"Recebendo evento - {evento.Pessoa.ToString()}");
            return pessoaComandoServico.AdicionarAsync(evento.Pessoa);
        }
    }
}
