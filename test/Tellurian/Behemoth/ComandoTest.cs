using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tellurian.Drone.Interfaces;
using Nebularium.Tellurian.Recursos;
using Nebularium.Tiamat.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Nebularium.Tellurian.Behemoth
{
    public class ComandoTest : TesteBase
    {
        private readonly IPessoaComandoRepositorio comandoRepositorio;
        private readonly IPessoaConsultaRepositorio consultaRepositorio;

        public ComandoTest(ITestOutputHelper saida) : base(saida)
        {
            comandoRepositorio = GestorDependencia.Instancia.ObterInstancia<IPessoaComandoRepositorio>();
            consultaRepositorio = GestorDependencia.Instancia.ObterInstancia<IPessoaConsultaRepositorio>();
        }

        [Fact]
        public async void AdicionarAsync_test()
        {
            var pessoaTeste = Pessoa.Pessoas[2];
            var pessoa = new Pessoa().Injete(pessoaTeste);
            pessoa.Id = null;
            Assert.Null(pessoa.Id);
            await comandoRepositorio.AdicionarAsync(pessoa);
            Assert.NotNull(pessoa);
            Assert.NotNull(pessoa.Id);
            Assert.NotEqual(pessoa.Id, pessoaTeste.Id);

            var pessoaConsulta = await consultaRepositorio.ObterAsync(pessoa.Id);
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

            await comandoRepositorio.AdicionarAsync(pessoas);

            pessoas.ForEach(async pessoa =>
            {
                Assert.NotNull(pessoa);
                Assert.NotNull(pessoa.Id);
                var pessoaTeste = Pessoa.Pessoas.FirstOrDefault(p => p.NomeSobrenome.Equals(pessoa.NomeSobrenome));
                Assert.NotNull(pessoaTeste);
                Assert.NotEqual(pessoa.Id, pessoaTeste.Id);
                var pessoaConsulta = await consultaRepositorio.ObterAsync(pessoa.Id);
                Assert.NotNull(pessoaConsulta);

                Assert.True(pessoa.IguaisMenosPorId(pessoaConsulta));
                Assert.True(pessoaConsulta.IguaisMenosPorId(pessoaTeste));
            });
        }

        [Fact]
        public async void AtualizarUmAsync_test()
        {
            var pessoaTeste = consultaRepositorio.ObterTodosQueryableAsync(query => query.Where(p => p.NomeSobrenome.Contains("Melissa")).Take(1)).Result.FirstOrDefault();

            var listaProps = PropriedadeValorFabrica<Pessoa>.Iniciar()
                                .Add(c => c.Genero, Genero.Indefinido, false)
                                .Add(c => c.Enderecos, new List<Endereco> { new Endereco { Cep = 12399, Cidade = "Catitu-mirim", Estado = "PA" } });
            await comandoRepositorio.AtualizarUmAsync(c => c.NomeSobrenome.Contains("Melissa"), listaProps.ObterTodos);

            var pessoaConsulta = await consultaRepositorio.ObterAsync(pessoaTeste.Id);
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
            var resultado = await comandoRepositorio.AtualizarMuitosAsync(c => c.Genero == Genero.Masculino, listaProps.ObterTodos);

            Assert.True(resultado);
            var pessoaConsulta = await consultaRepositorio.ObterTodosAsync(c => c.Genero == Genero.Indefinido);
            Assert.NotEmpty(pessoaConsulta);
        }

        [Fact]
        public async void AtualizarNaMaoAsync_test()
        {
            await comandoRepositorio.AtualizarNaMao();

            var pessoaConsulta = await consultaRepositorio.ObterTodosAsync(c => c.NomeSobrenome.Contains("Melissa"));
            Assert.NotEmpty(pessoaConsulta);
            var melissa = pessoaConsulta.FirstOrDefault();
            Assert.Equal(Genero.Indefinido, melissa.Genero);
            Assert.NotEmpty(melissa.Enderecos);
            //Assert.True(melissa.Enderecos.Any(e => e.Cidade == "Catitu-mirim"));
        }

        [Fact]
        public async void DeletarAsync_test()
        {
            var pessoaTeste = consultaRepositorio.ObterTodosQueryableAsync(query => query.Where(p => p.Id != null)).Result.LastOrDefault();
            Assert.NotNull(pessoaTeste);
            await comandoRepositorio.RemoverUmAsync(pessoaTeste.Id);

            var pessoaConsulta = await consultaRepositorio.ObterAsync(pessoaTeste.Id);
            Assert.Null(pessoaConsulta);
        }

        [Fact]
        public async void DeletarMuitosAsync_test()
        {
            await comandoRepositorio.RemoverMuitosAsync(c => c.Genero == Genero.Indefinido);

            var pessoaConsulta = await consultaRepositorio.ObterTodosAsync(p => p.Genero == Genero.Indefinido);
            Assert.Empty(pessoaConsulta);
        }

    }
}
