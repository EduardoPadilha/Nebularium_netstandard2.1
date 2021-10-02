using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nebularium.Behemoth.Mongo.Abstracoes;
using Nebularium.Behemoth.Mongo.Repositorios;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tellurian.Drone.Behemoth.Mapeamentos;
using Nebularium.Tellurian.Drone.Entidades;
using Nebularium.Tellurian.Drone.Interfaces.Repositorios;
using Nebularium.Tiamat.Excecoes;
using Nebularium.Tiamat.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nebularium.Tellurian.Drone.Behemoth.Repositorios
{
    public class PessoaRepositorio : RepositorioEntidade<Pessoa, PessoaMapeamento>, IPessoaRepositorio
    {
        public PessoaRepositorio(IMongoContexto context, ILogger<PessoaRepositorio> logger) : base(context, logger)
        {
        }

        public override IOrderedMongoQueryable<PessoaMapeamento> OrdernarPadrao(IMongoQueryable<PessoaMapeamento> query)
        {
            return query.OrderBy(c => c.Nascimento);
        }

        protected override string NomeColecao => typeof(Pessoa).Name.SnakeCase();

        public async Task AtualizarNaMao()
        {
            var builder = Builders<PessoaMapeamento>.Update
                .Set(c => c.Genero, Genero.Indefinido)
                .Set(c => c.Enderecos, new List<EnderecoMapeamento> { new EnderecoMapeamento { Cep = 12399, Cidade = "Catitu-mirim", Estado = "PA" } });
            await colecao.UpdateOneAsync(c => c.NomeSobrenome.Contains("Melissa"), builder);
        }

        protected async override Task ValidaUnicidade(Pessoa entidade)
        {
            var ativosComMesmaChave = await ObterTodosAsync(c => c.Cpf == entidade.Cpf && c.Id != entidade.Id);

            if (ativosComMesmaChave.AnySafe())
                throw new UnicidadeExcecao(entidade.Cpf);
        }

        protected async override Task ValidaUnicidadeAtualizacao(Expression<Func<Pessoa, bool>> predicado, List<PropriedadeValor> propriedades)
        {
            if (!propriedades.Any(c => c.Nome == nameof(Pessoa.Cpf))) return;
            var mutaveis = await ObterTodosAsync(predicado);
            if (!mutaveis.AnySafe()) return;

            var valor = (string)propriedades.FirstOrDefault(c => c.Nome == nameof(Pessoa.Cpf)).Valor;

            if (mutaveis.Count() > 1)
                throw new UnicidadeExcecao(valor);

            var comUnicidade = await ObterTodosAsync(c => c.Cpf == valor);

            if (comUnicidade.Count() > 1) throw new UnicidadeExcecao(valor);

            if (comUnicidade.Count() > 0 && !comUnicidade.Any(c => mutaveis.FirstOrDefault().Id == c.Id))
                throw new UnicidadeExcecao(valor);
        }
    }
}
