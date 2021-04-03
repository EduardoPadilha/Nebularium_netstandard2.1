using Nebularium.Tarrasque.Extensoes;
using System;

namespace Nebularium.Tiamat.Validacoes
{
    public static class ValidadoresSimples
    {
        public static Func<string, ValidacaoSimples> Id = id => new ValidacaoSimples("Id", "O Id não pode ser nulo", () => !id.LimpoNuloBranco());
    }
}
