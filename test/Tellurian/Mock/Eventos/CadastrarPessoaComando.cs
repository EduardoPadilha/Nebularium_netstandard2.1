using Nebularium.Weaver;

namespace Nebularium.Tellurian.Mock.Eventos
{
    public class CadastrarPessoaComando : Evento
    {
        public Pessoa Pessoa { get; }

        public CadastrarPessoaComando(Pessoa pessoa)
        {
            Pessoa = pessoa;
        }
    }
}
