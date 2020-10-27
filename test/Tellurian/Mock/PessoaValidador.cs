using FluentValidation;
using Nebularium.Tarrasque.Interfaces;
using Nebularium.Tiamat.Interfaces;
using Nebularium.Tiamat.Validacoes;

namespace Nebularium.Tellurian.Mock
{
    public class PessoaValidador : ValidadorAbstrato<Pessoa>
    {
        public PessoaValidador(IDisplayNameExtrator displayNameExtrator, IValidador<Endereco> enderecoValidador) : base(displayNameExtrator)
        {
            RuleFor(c => c.NomeSobrenome)
                .NotEmpty()
                .WithMessage("{PropertyName} não pode ser vazio")
                .MinimumLength(3)
                .MaximumLength(100);

            RuleFor(c => c.Enderecos)
                .NotNull()
                .WithMessage("{PropertyName} não pode ser vazio");

            RuleForEach(c => c.Enderecos).SetValidator((EnderecoValidador)enderecoValidador);
        }
    }

    public class EnderecoValidador : ValidadorAbstrato<Endereco>
    {
        public EnderecoValidador(IDisplayNameExtrator displayNameExtrator) : base(displayNameExtrator)
        {
            RuleFor(c => c.Numero)
                .NotEmpty()
                .WithMessage("{PropertyName} não pode ser vazio");

            RuleFor(c => c.Logradouro)
                .NotEmpty()
                .WithMessage("{PropertyName} não pode ser vazio");

            RuleFor(c => c.Cep)
                .NotEmpty()
                .WithMessage("{PropertyName} não pode ser vazio")
                .GreaterThan(99999999)
                .WithMessage("{PropertyName} CEP inválido");

            RuleFor(c => c.Cidade)
                .NotEmpty()
                .WithMessage("{PropertyName} não pode ser vazio");

            RuleFor(c => c.Estado)
                .NotEmpty()
                .WithMessage("{PropertyName} não pode ser vazio");
        }
    }
}
