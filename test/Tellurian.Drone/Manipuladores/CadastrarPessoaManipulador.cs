using Microsoft.Extensions.Logging;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tellurian.Drone.Eventos;
using Nebularium.Tellurian.Drone.Features;
using Nebularium.Weaver.Interfaces;
using System.Threading.Tasks;

namespace Nebularium.Tellurian.Drone.Manipuladores
{
    public class CadastrarPessoaManipulador : IManipuladorEvento<CadastrarPessoaComandoEvento>
    {
        private readonly ICadastrarPessoa cadastrarPessoa;
        private readonly ILogger<Pessoa> logger;
        public CadastrarPessoaManipulador(ICadastrarPessoa cadastrarPessoa, ILogger<Pessoa> logger)
        {
            this.cadastrarPessoa = cadastrarPessoa;
            this.logger = logger;
        }


        public Task Resolver(CadastrarPessoaComandoEvento evento)
        {
            logger.LogInformation($"Recebendo evento - {evento.Pessoa.ToString()}");
            return cadastrarPessoa.Executar(evento.Pessoa);
        }
    }
}
