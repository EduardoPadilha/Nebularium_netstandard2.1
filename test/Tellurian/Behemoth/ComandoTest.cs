using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tellurian.Drone.Interfaces.Repositorios;
using Nebularium.Tellurian.Recursos;
using Nebularium.Tiamat.Recursos;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Nebularium.Tellurian.Behemoth
{
    public class ComandoTest : TesteBase
    {
        private readonly IPessoaRepositorio repositorio;

        public ComandoTest(ITestOutputHelper saida) : base(saida)
        {
            repositorio = GestorDependencia.Instancia.ObterInstancia<IPessoaRepositorio>();
        }

        [Fact]
        public async void AdicionarAsync_test()
        {
            var pessoaTeste = Pessoa.Pessoas[2];
            var pessoa = new Pessoa().Injete(pessoaTeste);
            pessoa.Id = null;
            Assert.Null(pessoa.Id);
            await repositorio.AdicionarAsync(pessoa);
            Assert.NotNull(pessoa);
            Assert.NotNull(pessoa.Id);
            Assert.NotEqual(pessoa.Id, pessoaTeste.Id);

            var pessoaConsulta = await repositorio.ObterAsync(pessoa.Id);
            Assert.NotNull(pessoaConsulta);
        }

        [Fact]
        public async void AdicionarMuitosAsync_test()
        {
            var pessoas = new List<Pessoa>().Injete(Pessoa.Pessoas);
            pessoas.ForEach(pessoa =>
            {
                pessoa.Id = null;
            });
            Assert.True(pessoas.All(p => p.Id == null));

            await repositorio.AdicionarAsync(pessoas);

            pessoas.ForEach(async pessoa =>
            {
                Assert.NotNull(pessoa);
                Assert.NotNull(pessoa.Id);
                var pessoaTeste = Pessoa.Pessoas.FirstOrDefault(p => p.NomeSobrenome.Equals(pessoa.NomeSobrenome));
                Assert.NotNull(pessoaTeste);
                Assert.NotEqual(pessoa.Id, pessoaTeste.Id);
                var pessoaConsulta = await repositorio.ObterAsync(pessoa.Id);
                Assert.NotNull(pessoaConsulta);

                Assert.True(pessoa.IguaisMenosPorId(pessoaConsulta));
                Assert.True(pessoaConsulta.IguaisMenosPorId(pessoaTeste));
            });
        }

        [Fact]
        public async void AtualizarUmAsync_test()
        {
            var pessoaTeste = repositorio.ObterTodosAsync(p => p.NomeSobrenome.Contains("Melissa")).Result.FirstOrDefault();

            var listaProps = PropriedadeValorFabrica<Pessoa>.Iniciar()
                                .Add(c => c.Genero, Genero.Indefinido, false)
                                .Add(c => c.Enderecos, new List<Endereco> { new Endereco { Cep = 12399, Cidade = "Catitu-mirim", Estado = "PA" } });
            await repositorio.AtualizarUmAsync(c => c.NomeSobrenome.Contains("Melissa"), listaProps.ObterTodos);

            var pessoaConsulta = await repositorio.ObterAsync(pessoaTeste.Id);
            Assert.NotNull(pessoaConsulta);
            Assert.Equal(Genero.Indefinido, pessoaConsulta.Genero);
            Assert.NotEmpty(pessoaConsulta.Enderecos);
        }

        [Fact]
        public async void AtualizarMuitosAsync_test()
        {
            var listaProps = PropriedadeValorFabrica<Pessoa>.Iniciar()
                                        .Add(c => c.Genero, Genero.Indefinido, false)
                                        .Add(c => c.Enderecos, new List<Endereco> { new Endereco { Cep = 11111 } });
            var resultado = await repositorio.AtualizarMuitosAsync(c => c.Genero == Genero.Masculino, listaProps.ObterTodos);

            Assert.True(resultado);
            var pessoaConsulta = await repositorio.ObterTodosAsync(c => c.Genero == Genero.Indefinido);
            Assert.NotEmpty(pessoaConsulta);
        }

        [Fact]
        public async void AtualizarNaMaoAsync_test()
        {
            await repositorio.AtualizarNaMao();

            var pessoaConsulta = await repositorio.ObterTodosAsync(c => c.NomeSobrenome.Contains("Melissa"));
            Assert.NotEmpty(pessoaConsulta);
            var melissa = pessoaConsulta.FirstOrDefault();
            Assert.Equal(Genero.Indefinido, melissa.Genero);
            Assert.NotEmpty(melissa.Enderecos);
            //Assert.True(melissa.Enderecos.Any(e => e.Cidade == "Catitu-mirim"));
        }

        [Fact]
        public async void DeletarAsync_test()
        {
            var pessoaTeste = repositorio.ObterTodosAsync(p => p.Id != null).Result.LastOrDefault();
            Assert.NotNull(pessoaTeste);
            await repositorio.RemoverUmAsync(pessoaTeste.Id);

            var pessoaConsulta = await repositorio.ObterAsync(pessoaTeste.Id);
            Assert.Null(pessoaConsulta);
        }

        [Fact]
        public async void DeletarMuitosAsync_test()
        {
            await repositorio.RemoverMuitosAsync(c => c.Genero == Genero.Indefinido);

            var pessoaConsulta = await repositorio.ObterTodosAsync(p => p.Genero == Genero.Indefinido);
            Assert.Empty(pessoaConsulta);
        }

    }
}
