using FluentValidation;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tellurian.Mock;
using Nebularium.Tellurian.Recursos;
using Nebularium.Tiamat.Interfaces;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Nebularium.Tellurian.Tiamat
{
    public class ValidadorTest : TesteBase
    {
        private readonly IValidador<Pessoa> pessoaValidador;
        private readonly IValidador<Endereco> enderecoValidador;
        public ValidadorTest(ITestOutputHelper saida) : base(saida)
        {
            pessoaValidador = GestorDependencia.Instancia.ObterInstancia<IValidador<Pessoa>>();
            enderecoValidador = GestorDependencia.Instancia.ObterInstancia<IValidador<Endereco>>();
        }

        [Fact]
        public void ValidacaoPessoa()
        {
            Pessoa.PessoasValidacao.ForEach(pessoa =>
            {
                var validacao = pessoaValidador.Validar(pessoa);
                Console.WriteLine($"=> {pessoa.NomeSobrenome} - [{validacao.Valido}]");
                foreach (var erro in validacao.Erros)
                    Console.WriteLine($"{erro.NomePropriedade}: {erro.Mensagem}");
                Console.WriteLine($"========================>\n");
            });
        }

        [Fact]
        public void ValidacaoEndereco()
        {
            Pessoa.PessoasValidacao.Where(c => c.Enderecos != null).SelectMany(e => e.Enderecos).ToList().ForEach(endereco =>
            {
                var validacao = enderecoValidador.Validar(endereco);
                Console.WriteLine($"=> {endereco.Cep} - [{validacao.Valido}]");
                foreach (var erro in validacao.Erros)
                    Console.WriteLine($"{erro.NomePropriedade}: {erro.Mensagem}");
                Console.WriteLine($"========================>\n");
            });
        }
    }
}
