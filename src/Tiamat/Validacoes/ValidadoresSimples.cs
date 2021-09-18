using Nebularium.Tarrasque.Extensoes;
using System;

namespace Nebularium.Tiamat.Validacoes
{
    public static class ValidadoresSimples
    {
        public static Func<string, ValidacaoSimples> Id = id => new ValidacaoSimples("Id", "O Id não pode ser nulo", () => !id.LimpoNuloBranco());
        public static Func<string, string, ValidacaoSimples> CampoVazio =
            (nome, valor) => new ValidacaoSimples(nome, "Campo não pode ser nulo ou vazio", () => !valor.LimpoNuloBranco());

        public static Func<string, Guid, ValidacaoSimples> Guid = (nome, guid) =>
                new ValidacaoSimples(nome, "Campo não pode ser nulo ou vazio", () => guid != default);
    }
}
