using Microsoft.Extensions.Logging;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Entidades;
using Nebularium.Tiamat.Excecoes;
using Nebularium.Tiamat.Validacoes;
using System;

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
            validadorSimples.EventoFalhaValidacao = erros => throw new ValidacaoExcecao(erros);
        }
    }
}
