using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tellurian.Drone.Filtros;
using Nebularium.Tellurian.Recursos;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Xunit.Abstractions;
using Nebularium.Tiamat.Recursos;
using Nebularium.Tellurian.Drone.Interfaces.Repositorios;

namespace Nebularium.Tellurian.Behemoth
{
    public class ConsultaTest : TesteBase
    {
        private readonly IPessoaConsultaRepositorio repositorio;
        public ConsultaTest(ITestOutputHelper saida) : base(saida)
        {
            repositorio = GestorDependencia.Instancia.ObterInstancia<IPessoaConsultaRepositorio>();
        }

        [Fact]
        public async void ObterAsync_test()
        {
            var id = "6068f6f4e76288094363064b";
            var entidade = await repositorio.ObterAsync(id);
            Assert.NotNull(entidade);
        }

        [Fact]
        public async void ObterTodosAsync_PorFiltro_test()
        {
            var criterio = new Pessoa { NomeSobrenome = "Almada" };
            var filtro = new PessoaFiltro();
            filtro.DataInicio = new DateTime(1900, 1, 1);
            filtro.DataFim = new DateTime(1990, 1, 1);
            filtro.SetarModeloCriterio(criterio);

            var todos = await repositorio.ObterTodosAsync(filtro);
            Assert.NotNull(todos);
            Assert.Single(todos);
        }

        [Fact]
        public async void ObterTodosAsync_PorPredicado_test()
        {
            Expression<Func<Pessoa, bool>> predicadoPorNome = pessoa => pessoa.NomeSobrenome.Contains("Pinto");
            Expression<Func<Pessoa, bool>> predicadoPorGenero = pessoa => pessoa.Genero == Genero.Feminio;
            Expression<Func<Pessoa, bool>> predicadoPorNascimento = pessoa => pessoa.Nascimento > new DateTime(1900, 1, 1) && pessoa.Nascimento < new DateTime(1950, 1, 1);
            var predicado = predicadoPorGenero.And(predicadoPorNascimento).And(predicadoPorNome);

            var todos = await repositorio.ObterTodosAsync(predicado);
            Assert.NotNull(todos);
            Assert.Single(todos);
        }

        [Fact]
        public async void ObterTodosAsync_PorPredicadoIQueryable_test()
        {
            Expression<Func<IQueryable<Pessoa>, IQueryable<Pessoa>>> predicado =
                pessoas => pessoas.Where(pessoa => pessoa.NomeSobrenome.Contains("Pinto") &&
                                                    pessoa.Genero == Genero.Feminio &&
                                                    pessoa.Nascimento > new DateTime(1900, 1, 1) && pessoa.Nascimento < new DateTime(1950, 1, 1));

            var todos = await repositorio.ObterTodosQueryableAsync(predicado);
            Assert.NotNull(todos);
            Assert.Single(todos);
        }

        [Fact]
        public async void ObterTodosAsync_Paginado_test()
        {
            var paginador = new Paginador { Pagina = 1 };
            var todos = await repositorio.ObterTodosAsync(c => true, paginador);
            Assert.NotNull(todos);
            Assert.True(paginador.TotalRegistros > 1);
        }
    }
}
