using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tellurian.Mock;
using Nebularium.Tellurian.Recursos;
using Xunit;
using Xunit.Abstractions;

namespace Nebularium.Tellurian.Tiamat.Mock
{
    public class DisplayNameTest : TesteBase
    {
        public DisplayNameTest(ITestOutputHelper saida) : base(saida)
        {
        }

        [Fact]
        public void GestorConfiguracao_test()
        {
            saida.WriteLine(nameof(Pessoa));
            Pessoa p = new Pessoa();
            Endereco e = new Endereco();
            saida.WriteLine($"Saída: {p.ObterDisplay(prop => prop.Id)} - {p.ObterDisplay(prop => prop.NomeSobrenome)} - {p.ObterDisplay(prop => prop.Genero)}");
            Assert.Equal("CPF", p.ObterDisplay(prop => prop.Id));
            Assert.Equal("Nome Sobrenome", p.ObterDisplay(prop => prop.NomeSobrenome));
            Assert.Equal("Gênero", p.ObterDisplay(prop => prop.Genero));
            Assert.Equal("Endereços", p.ObterDisplay(prop => prop.Enderecos));

            Assert.Equal("Número", e.ObterDisplay(prop => prop.Numero));
            Assert.Equal("Complemento (Apartamento, bloco, etc..)", e.ObterDisplay(prop => prop.Complemento));
            Assert.Equal("Logradouro (Rua, Avenida, etc...)", e.ObterDisplay(prop => prop.Logradouro));
            Assert.Equal("CEP", e.ObterDisplay(prop => prop.Cep));
            Assert.Equal("Cidade/Província", e.ObterDisplay(prop => prop.Cidade));
            Assert.Equal("Estado (UF)", e.ObterDisplay(prop => prop.Estado));

        }
    }
}
