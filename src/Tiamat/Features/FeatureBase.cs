using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Recursos;
using Nebularium.Tiamat.Validacoes;
using System;

namespace Nebularium.Tiamat.Features
{
    public abstract class FeatureBase
    {
        protected readonly IContextoNotificacao notificacao;
        protected ValidadorSimples validadorSimples;

        protected FeatureBase(IContextoNotificacao notificacao)
        {
            this.notificacao = notificacao ?? throw new ArgumentException("notificacao");
            validadorSimples = ValidadorSimples.Iniciar();
            validadorSimples.EventoFalhaValidacao = erros => notificacao.AddErros(erros);
        }

        protected bool ValidarSimples()
        {
            return validadorSimples.Validar();
        }
    }
    public abstract class FeatureValidando<TInbound> : FeatureBase where TInbound : class
    {

        protected readonly IValidador<TInbound> validador;
        protected FeatureValidando(IContextoNotificacao notificacao, IValidador<TInbound> validador) : base(notificacao)
        {
            this.validador = validador ?? throw new ArgumentException("validador");
            validador.EventoFalhaValidacao = erros => notificacao.AddErros(erros);
        }

        protected bool ValidarInbound(TInbound inbound)
        {
            var validacaoResultado = validador.Validar(inbound);
            return validacaoResultado.Valido;
        }

        protected bool ValidarTudo(TInbound inbound)
        {
            var validacaoResultado = validador.Validar(inbound);
            return validacaoResultado.Valido && ValidarSimples();
        }
    }
    public abstract class FeatureConsulta<TEntidade> : FeatureBase
         where TEntidade : IEntidade, new()
    {
        protected readonly IConsultaRepositorioBase<TEntidade> repositorio;
        protected FeatureConsulta(IContextoNotificacao notificacao,
            IConsultaRepositorioBase<TEntidade> repositorio) : base(notificacao)
        {
            this.repositorio = repositorio ?? throw new ArgumentException("repositorio");
        }

        public IPaginador ObterPaginador(IPaginacao paginacao)
        {
            return paginacao.Como<Paginador>();
        }
    }
    public abstract class FeatureComando<TEntidade> : FeatureBase
         where TEntidade : IEntidade, new()
    {
        protected readonly IComandoRepositorioBase<TEntidade> repositorio;
        protected FeatureComando(IContextoNotificacao notificacao,
            IComandoRepositorioBase<TEntidade> repositorio) : base(notificacao)
        {
            this.repositorio = repositorio ?? throw new ArgumentException("repositorio");
        }
    }
    public abstract class FeatureConsultaValidando<TEntidade, TInbound> :
        FeatureValidando<TInbound>
        where TInbound : class
        where TEntidade : IEntidade, new()
    {

        protected readonly IConsultaRepositorioBase<TEntidade> repositorio;
        protected FeatureConsultaValidando(IContextoNotificacao notificacao,
            IValidador<TInbound> validador, IConsultaRepositorioBase<TEntidade> repositorio) :
            base(notificacao, validador)
        {
            this.repositorio = repositorio ?? throw new ArgumentException("repositorio");
        }

        public IPaginador ObterPaginador(IPaginacao paginacao)
        {
            return paginacao.Como<Paginador>();
        }
    }
    public abstract class FeatureComandoValidando<TEntidade, TInbound> :
        FeatureValidando<TInbound>
        where TInbound : class
        where TEntidade : IEntidade, new()
    {

        protected readonly IComandoRepositorioBase<TEntidade> repositorio;
        protected FeatureComandoValidando(IContextoNotificacao notificacao,
            IValidador<TInbound> validador, IComandoRepositorioBase<TEntidade> repositorio) :
            base(notificacao, validador)
        {
            this.repositorio = repositorio ?? throw new ArgumentException("repositorio");
        }
    }
}
