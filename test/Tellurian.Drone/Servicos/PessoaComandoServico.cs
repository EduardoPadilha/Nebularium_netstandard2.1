using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tiamat.Interfaces;
using Nebularium.Tiamat.Servicos;

namespace Nebularium.Tellurian.Drone.Servicos
{
    public class PessoaComandoServico : ComandoServico<Pessoa>
    {
        public PessoaComandoServico(IComandoRepositorio<Pessoa> repositorioComando) : base(repositorioComando)
        {
        }
    }
}
