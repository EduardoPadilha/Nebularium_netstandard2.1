using FluentValidation;
using Nebularium.Tarrasque.Extensoes;

namespace Nebularium.Tiamat.Extensoes
{
    public static class FluentValidationExtensao
    {
        public static IRuleBuilderOptions<T, string> CnpjValido<T>(this IRuleBuilder<T, string> ruleBuilder)
        {

            return ruleBuilder.Must(rootObject =>
            {
                return rootObject.ValidarCnpj();
            })
            .WithMessage("{PropertyName} não é válido.");
        }
    }
}
