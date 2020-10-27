using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tarrasque.Interfaces;
using Nebularium.Tiamat.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Nebularium.Tiamat.Validacoes
{
    public abstract class ValidadorAbstrato<TEntidade> : AbstractValidator<TEntidade>, IValidador<TEntidade> where TEntidade : class
    {
        protected bool configurado;
        protected ValidadorAbstrato(IDisplayNameExtrator displayNameExtrator)
        {
            ValidatorOptions.Global.DisplayNameResolver = (tipo, membro, exp) => displayNameExtrator.ObterDisplay(tipo, membro);
        }
        public virtual ValidacaoResultado Validar(TEntidade entidade)
        {
            ValidationResult r = this.Validate(entidade);
            var resultado = r.Como<ValidacaoResultado>();
            return resultado;
        }
        public virtual ValidacaoResultado Validar(TEntidade entidade, string rulerSet)
        {
            ValidationResult r = this.Validate(entidade, options => options.IncludeRuleSets(rulerSet));
            var resultado = r.Como<ValidacaoResultado>();
            return resultado;
        }
        public virtual ValidacaoResultado ValidarPropriedade(TEntidade entidade, Expression<Func<TEntidade, object>> propriedade)
        {
            ValidationResult r = this.Validate(entidade, options => options.IncludeProperties(propriedade));
            var resultado = r.Como<ValidacaoResultado>();
            return resultado;
        }
        public virtual List<PropertyRule> PegarRegras()
        {
            var regras = CreateDescriptor();
            var propertyInfos = typeof(TEntidade).GetRuntimeProperties();
            var validadores = propertyInfos.SelectMany(inf => regras.GetRulesForMember(inf.Name).Select(r => (PropertyRule)r)).ToList();
            return validadores;
        }
    }
}
