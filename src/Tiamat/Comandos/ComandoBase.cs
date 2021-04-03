using Microsoft.Extensions.Logging;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Atributos;
using Nebularium.Tiamat.Entidades;
using Nebularium.Tiamat.Excecoes;
using Nebularium.Tiamat.Validacoes;
using System;
using System.Collections.Generic;

namespace Nebularium.Tiamat.Comandos
{

    public abstract class ComandoBase<TEntidade> where TEntidade : Entidade, new()
    {
        protected readonly ILogger logger;
        protected readonly IContextoNotificacao notificacao;
        protected readonly IComandoRepositorioBase<TEntidade> repositorio;
        protected IValidador<TEntidade> validador;
        protected ValidadorSimples validadorSimples;
        public ComandoBase(IContextoNotificacao notificacao,
            IComandoRepositorioBase<TEntidade> repositorio,
            ILogger<TEntidade> logger)
        {
            this.notificacao = notificacao ?? throw new ArgumentException("notificacao");
            this.repositorio = repositorio ?? throw new ArgumentException("repositorio");
            this.logger = logger ?? throw new ArgumentException("logger");
            validadorSimples = ValidadorSimples.Iniciar();
        }
        protected virtual void ValidarParametro(string metodo)
        {
            if (!GetType().TemAnotacao<ValidarParametroAttribute>(metodo)) return;

            var atributo = GetType().ObterAnotacao<ValidarParametroAttribute>(metodo);
            var temValidador = !atributo.NuloOuDefault();
            if (!temValidador) return;
            var config = new ConfigValidacao();
            if (temValidador)
                config.configurar(atributo.Validador.GetType(), atributo.Cenario, atributo.SoltarExcecao);

            var validadorInstancia = GestorDependencia.Instancia.ObterInstancia(config.Tipo);
            if (validadorInstancia == null)
            {
                LogarOuSoltarExcecao($"Nenhum validador para {typeof(TEntidade).GetType().Name}", config.SoltarExcecao);
                return;
            }

            if (!(validadorInstancia is IValidador<TEntidade>))
            {
                LogarOuSoltarExcecao($"Validador ${config.Tipo.Name} não é um validador válido para a entidade {typeof(TEntidade).GetType().Name}", config.SoltarExcecao);
                return;
            }
        }
        protected virtual void Validar(string metodo, TEntidade entidade)
        {
            if (GetType().TemAnotacao<NaoValidarAttribute>(metodo)) return;

            if (GetType().TemAnotacao<ValidacaoSimplesAttribute>(metodo))
            {
                ValidarSimples();
                return;
            }

            var validadorMetodo = GetType().ObterAnotacao<ValidadorAttribute>(metodo);
            var validadorClass = GetType().ObterAnotacao<ValidadorAttribute>();
            var temValidadorClass = !validadorClass.NuloOuDefault();
            var temValidarMetodo = !validadorMetodo.NuloOuDefault();
            if (!temValidadorClass && !temValidarMetodo) return;
            var config = new ConfigValidacao();
            if (temValidadorClass)
                config.configurar(validadorClass);
            if (temValidarMetodo)
                config.configurar(validadorMetodo);

            var validadorInstancia = GestorDependencia.Instancia.ObterInstancia(config.Tipo);
            if (validadorInstancia == null && !validadorSimples.TemValidacao)
            {
                LogarOuSoltarExcecao($"Nenhum validador para {typeof(TEntidade).GetType().Name}", config.SoltarExcecao);
                return;
            }

            if (!validadorSimples.TemValidacao && !(validadorInstancia is IValidador<TEntidade>))
            {
                LogarOuSoltarExcecao($"Validador ${config.Tipo.Name} não é um validador válido para a entidade {typeof(TEntidade).GetType().Name}", config.SoltarExcecao);
                return;
            }

            validador = (IValidador<TEntidade>)validadorInstancia;
            Validar(entidade, config.Cenario, config.SoltarExcecao);
        }
        private void LogarOuSoltarExcecao(string erro, bool soltarExcecao)
        {
            logger.LogWarning(erro);
            if (soltarExcecao)
                throw new Exception(erro);
        }


        #region Validação

        protected virtual void Validar(TEntidade entidade, string[] ruleset = null, bool comExcecao = true)
        {
            if (comExcecao)
                ValidarComExcecao(entidade, ruleset);
            else
                ValidarComNotificacao(entidade, ruleset);
        }

        protected virtual void ValidarComExcecao(TEntidade entidade, string[] ruleset = null)
        {
            var validacao = ValidarTudo(entidade, ruleset);
            if (!validacao.Valido)
                throw new ValidacaoExcecao(validacao.Erros);
        }

        protected virtual bool ValidarComNotificacao(TEntidade entidade, string[] ruleset = null)
        {
            validador.EventoFalhaValidacao = erros => notificacao.AddErros(erros);
            var resultado = ValidarTudo(entidade, ruleset);
            return resultado.Valido;
        }

        private ValidacaoResultado ValidarTudo(TEntidade entidade, string[] ruleset = null)
        {
            return ruleset.AnySafe() ? validador.Validar(entidade, validadorSimples, ruleset) : validador.Validar(entidade, validadorSimples); ;
        }

        private ValidacaoResultado ValidarSimples()
        {
            var resultado = new ValidacaoResultado { Erros = new List<ErroValidacao>() };
            if (!validadorSimples.TemValidacao) return resultado;

            validadorSimples.EventoFalhaValidacao = erros => resultado.Erros.AddRange(erros.ConvertAll(simples =>
                new ErroValidacao { NomePropriedade = simples.Campo, Mensagem = simples.Mensagem }));
            validadorSimples.Validar();

            return resultado;
        }


        #endregion
    }

    internal class ConfigValidacao
    {
        public void configurar(ValidadorAttribute atributo)
        {
            configurar(atributo.Tipo, atributo.Cenario, atributo.SoltarExcecao);
        }
        public void configurar(Type tipo, string[] cenario, bool soltarExcecao)
        {
            if (!tipo.NuloOuDefault())
                Tipo = tipo;
            Cenario = cenario;
            SoltarExcecao = soltarExcecao;
        }

        public Type Tipo { get; set; }
        public string[] Cenario { get; set; }
        public bool SoltarExcecao { get; set; }
    }
}
