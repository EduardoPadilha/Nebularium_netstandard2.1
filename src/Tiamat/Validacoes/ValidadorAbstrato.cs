using FluentValidation;
using FluentValidation.Results;
using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tiamat.Abstracoes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace Nebularium.Tiamat.Validacoes
{
    public abstract class ValidadorAbstrato<TEntidade> : AbstractValidator<TEntidade>, IValidador<TEntidade> where TEntidade : class
    {
        protected ValidadorAbstrato(IDisplayNameExtrator displayNameExtrator)
        {
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("pt-BR");
            ValidatorOptions.Global.DisplayNameResolver = (tipo, membro, exp) => displayNameExtrator.ObterDisplay(tipo, membro);
        }
        public Action<List<ErroValidacao>> EventoFalhaValidacao { get; set; }
        public virtual ValidacaoResultado Validar(TEntidade entidade)
        {
            ValidationResult r = Validate(entidade);
            var resultado = ConverteValidacaoResultado(r);
            DisparaEventoFalhaValidacao(resultado);
            return resultado;
        }
        public virtual ValidacaoResultado Validar(TEntidade entidade, params string[] rulerSet)
        {
            ValidationResult r = this.Validate(entidade, options => options.IncludeRuleSets(rulerSet));
            var resultado = ConverteValidacaoResultado(r);
            DisparaEventoFalhaValidacao(resultado);
            return resultado;
        }
        public virtual ValidacaoResultado ValidarPropriedade(TEntidade entidade, Expression<Func<TEntidade, object>> propriedade)
        {
            ValidationResult r = this.Validate(entidade, options => options.IncludeProperties(propriedade));
            var resultado = ConverteValidacaoResultado(r);
            DisparaEventoFalhaValidacao(resultado);
            return resultado;
        }

        private void DisparaEventoFalhaValidacao(ValidacaoResultado validacaoResultado)
        {
            if (validacaoResultado.Valido || EventoFalhaValidacao == null) return;

            EventoFalhaValidacao.Invoke(validacaoResultado.Erros);
        }

        private static ValidacaoResultado ConverteValidacaoResultado(ValidationResult resultado)
        {
            if (resultado.IsValid)
                return ValidacaoResultado.ResultadoValido;

            var erros = resultado.Errors.ToList().ConvertAll(erro => new ErroValidacao(erro.PropertyName, erro.ErrorMessage));
            var convertido = new ValidacaoResultado(erros);
            return convertido;
        }
    }
}
