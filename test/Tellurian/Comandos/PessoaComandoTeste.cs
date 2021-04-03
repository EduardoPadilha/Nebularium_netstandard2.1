using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tellurian.Drone.Interfaces.Comandos;
using Nebularium.Tellurian.Drone.Interfaces.Repositorios;
using Nebularium.Tellurian.Recursos;
using Xunit;
using Xunit.Abstractions;

namespace Nebularium.Tellurian.Comandos
{
    public class PessoaComandoTeste : TesteBase
    {
        private readonly IPessoaComandos comandos;
        private readonly IPessoaConsultaRepositorio consultaRepositorio;

        public PessoaComandoTeste(ITestOutputHelper saida) : base(saida)
        {
            comandos = GestorDependencia.Instancia.ObterInstancia<IPessoaComandos>();
            consultaRepositorio = GestorDependencia.Instancia.ObterInstancia<IPessoaConsultaRepositorio>();
        }

        [Fact]
        public async void AdicionarAsync_test()
        {
            var pessoaTeste = Pessoa.Pessoas[2];
            var pessoa = new Pessoa().Injete(pessoaTeste);
            pessoa.Id = null;
            Assert.Null(pessoa.Id);
            await comandos.AdicionarUmAsync(pessoa);
            Assert.NotNull(pessoa);
            Assert.NotNull(pessoa.Id);
            Assert.NotEqual(pessoa.Id, pessoaTeste.Id);

            var pessoaConsulta = await consultaRepositorio.ObterAsync(pessoa.Id);
            Assert.NotNull(pessoaConsulta);
        }
    }
}
