using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Extensoes;
using Nebularium.Tiamat.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Nebularium.Tiamat.Validacoes
{
    public abstract class ValidadorAbstrato<TEntidade> : AbstractValidator<TEntidade>, IValidador<TEntidade> where TEntidade : class
    {
        protected ValidadorAbstrato(IDisplayNameExtrator displayNameExtrator)
        {
            ValidatorOptions.Global.DisplayNameResolver = (tipo, membro, exp) => displayNameExtrator.ObterDisplay(tipo, membro);
        }
        public Action<List<ErroValidacao>> EventoFalhaValidacao { get; set; }
        public virtual ValidacaoResultado Validar(TEntidade entidade, ValidadorSimples validadorSimples = null)
        {
            ValidationResult r = Validate(entidade);
            var resultado = r.Como<ValidacaoResultado>();
            if (validadorSimples != null)
                resultado = ValidarComValidadorSimples(validadorSimples, resultado);
            DisparaEventoFalhaValidacao(resultado);
            return resultado;
        }
        public virtual ValidacaoResultado Validar(TEntidade entidade, ValidadorSimples validadorSimples, params string[] rulerSet)
        {
            ValidationResult r = this.Validate(entidade, options => options.IncludeRuleSets(rulerSet));
            var resultado = r.Como<ValidacaoResultado>();
            resultado = ValidarComValidadorSimples(validadorSimples, resultado);
            DisparaEventoFalhaValidacao(resultado);
            return resultado;
        }
        public virtual ValidacaoResultado Validar(TEntidade entidade, params string[] rulerSet)
        {
            ValidationResult r = this.Validate(entidade, options => options.IncludeRuleSets(rulerSet));
            var resultado = r.Como<ValidacaoResultado>();
            DisparaEventoFalhaValidacao(resultado);
            return resultado;
        }
        public virtual ValidacaoResultado Validar(List<PropriedadeValor> valores, ValidadorSimples validadorSimples = null)
        {
            var entidade = valores.ParaEntidade<TEntidade>();
            return Validar(entidade, validadorSimples);
        }
        public virtual ValidacaoResultado Validar(List<PropriedadeValor> valores, ValidadorSimples validadorSimples, params string[] rulerSet)
        {
            var entidade = valores.ParaEntidade<TEntidade>();
            return Validar(entidade, validadorSimples, rulerSet);
        }
        public virtual ValidacaoResultado Validar(List<PropriedadeValor> valores, params string[] rulerSet)
        {
            var entidade = valores.ParaEntidade<TEntidade>();
            return Validar(entidade, rulerSet);
        }
        public virtual ValidacaoResultado ValidarPropriedade(TEntidade entidade, Expression<Func<TEntidade, object>> propriedade)
        {
            ValidationResult r = this.Validate(entidade, options => options.IncludeProperties(propriedade));
            var resultado = r.Como<ValidacaoResultado>();
            DisparaEventoFalhaValidacao(resultado);
            return resultado;
        }
        public virtual List<PropertyRule> PegarRegras()
        {
            var regras = CreateDescriptor();
            var propertyInfos = typeof(TEntidade).GetRuntimeProperties();
            var validadores = propertyInfos.SelectMany(inf => regras.GetRulesForMember(inf.Name).Select(r => (PropertyRule)r)).ToList();
            return validadores;
        }

        private void DisparaEventoFalhaValidacao(ValidacaoResultado validacaoResultado)
        {
            if (validacaoResultado.Valido || EventoFalhaValidacao == null) return;

            EventoFalhaValidacao.Invoke(validacaoResultado.Erros);
        }

        private ValidacaoResultado ValidarComValidadorSimples(ValidadorSimples validadorSimples, ValidacaoResultado resultado)
        {
            if (!validadorSimples.TemValidacao) return resultado;

            if (resultado == null)
                resultado = new ValidacaoResultado { Erros = new List<ErroValidacao>() };

            validadorSimples.EventoFalhaValidacao = erros => resultado.Erros.AddRange(erros.ConvertAll(simples =>
                new ErroValidacao { NomePropriedade = simples.Campo, Mensagem = simples.Mensagem }));
            validadorSimples.Validar();

            return resultado;
        }
    }
}
