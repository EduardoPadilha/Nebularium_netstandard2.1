using Nebularium.Tellurian.Mock;
using Nebularium.Tellurian.Recursos;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Nebularium.Tellurian.Tiamat
{
    public class FiltroTest : TesteBase
    {
        public FiltroTest(ITestOutputHelper saida) : base(saida)
        {
        }

        [Fact]
        public void FiltroPessoa()
        {
            var filtro = new PessoaFiltro();
            filtro.Genero = Genero.Masculino;
            var pessoas = filtro.ObterFiltro(Pessoa.Pessoas.AsQueryable()).ToList();
            Assert.Equal(2, pessoas.Count());

            filtro = new PessoaFiltro();
            var criterio = new Pessoa
            {
                NomeSobrenome = "Stefany"
            };
            filtro.SetarModeloCriterio(criterio);
            pessoas = filtro.ObterFiltro(Pessoa.Pessoas.AsQueryable()).ToList();
            Assert.Single(pessoas);

            filtro = new PessoaFiltro();
            filtro.Cidade = "ca";
            pessoas = filtro.ObterFiltro(Pessoa.Pessoas.AsQueryable()).ToList();
            Assert.Equal(2, pessoas.Count());

            filtro = new PessoaFiltro();
            filtro.Cep = 69090690;
            pessoas = filtro.ObterFiltro(Pessoa.Pessoas.AsQueryable()).ToList();
            Assert.Single(pessoas);
        }
    }
}
