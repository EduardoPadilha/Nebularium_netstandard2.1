using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Weaver;

namespace Nebularium.Tellurian.Drone.Eventos
{
    public class CadastrarPessoaComandoEvento : Evento
    {
        public Pessoa Pessoa { get; }

        public CadastrarPessoaComandoEvento(Pessoa pessoa)
        {
            Pessoa = pessoa;
        }
    }
}
