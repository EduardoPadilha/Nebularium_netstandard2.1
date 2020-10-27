using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tellurian.Mock;
using Nebularium.Tellurian.Mock.Interfaces;
using Nebularium.Tellurian.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            var pessoaTeste = Pessoa.Pessoas[0];
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
        public async void AtualizarAsync_test()
        {
            var pessoaTeste = consultaRepositorio.ObterTodosAsync(query => query.Where(p => p.NomeSobrenome != null).Take(1)).Result.FirstOrDefault();
            var pessoa = new Pessoa().Injete(pessoaTeste);
            string pattern = @"[\d]";
            var match = Regex.Match(pessoa.NomeSobrenome, pattern);

            if (match.Success)
                pessoa.NomeSobrenome = Regex.Replace(pessoa.NomeSobrenome, pattern, $"{int.Parse(match.Value) + 1}");
            else
                pessoa.NomeSobrenome += " [1]";

            Assert.NotEqual(pessoaTeste.NomeSobrenome, pessoa.NomeSobrenome);
            if (pessoa.Nascimento == default)
                pessoa.Nascimento = new DateTime(1990, 1, 1);
            await comandoRepositorio.AtualizarAsync(pessoa);
            Assert.NotNull(pessoa);

            var pessoaConsulta = await consultaRepositorio.ObterAsync(pessoa.Id);
            Assert.NotNull(pessoaConsulta);
            Assert.Equal(pessoa.NomeSobrenome, pessoaConsulta.NomeSobrenome);
            Assert.NotEqual(pessoaTeste.NomeSobrenome, pessoaConsulta.NomeSobrenome);
        }

        [Fact]
        public async void DeletarAsync_test()
        {
            var pessoaTeste = consultaRepositorio.ObterTodosAsync(query => query.Where(p => p.Id != null)).Result.LastOrDefault();
            Assert.NotNull(pessoaTeste);
            await comandoRepositorio.RemoverAsync(pessoaTeste);

            var pessoaConsulta = await consultaRepositorio.ObterAsync(pessoaTeste.Id);
            Assert.Null(pessoaConsulta);
        }

        [Fact]
        public async void DeletarMuitosAsync_test()
        {
            var pessoasTeste = consultaRepositorio.ObterTodosAsync(query => query.Where(p => p.Id != null)).Result.TakeLast(2);
            Assert.NotNull(pessoasTeste);
            Assert.NotEmpty(pessoasTeste);
            var ids = pessoasTeste.Select(p => p.Id);
            await comandoRepositorio.RemoverAsync(pessoasTeste);

            var pessoaConsulta = await consultaRepositorio.ObterTodosAsync(p => ids.Contains(p.Id));
            Assert.Empty(pessoaConsulta);
        }

    }
}
