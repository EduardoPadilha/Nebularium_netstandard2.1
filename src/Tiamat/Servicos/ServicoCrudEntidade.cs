using Microsoft.Extensions.Logging;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Entidades;
using Nebularium.Tiamat.Excecoes;
using Nebularium.Tiamat.Recursos;
using Nebularium.Tiamat.Validacoes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nebularium.Tiamat.Servicos
{
    public abstract class ServicoCrudEntidade<TEntidade> : IServicoCrudEntidade<TEntidade> where TEntidade : Entidade, new()
    {
        protected readonly IComandoRepositorio<TEntidade> comandoRepositorio;
        protected readonly IConsultaRepositorioBase<TEntidade> consultaRepositorio;
        protected readonly ILogger<TEntidade> log;
        protected readonly ValidadorSimples validadorSimples;
        protected readonly IValidador<TEntidade> validador;

        protected ServicoCrudEntidade(IComandoRepositorio<TEntidade> comandoRepositorio,
            IConsultaRepositorioBase<TEntidade> consultaRepositorio,
            ILogger<TEntidade> log,
            IValidador<TEntidade> validador)
        {
            this.comandoRepositorio = comandoRepositorio;
            this.consultaRepositorio = consultaRepositorio;
            this.log = log;
            validadorSimples = new ValidadorSimples
            {
                EventoFalhaValidacao = erros => throw new ValidacaoExcecao(erros)
            };
            this.validador = validador;
            this.validador.EventoFalhaValidacao = erros => throw new ValidacaoExcecao(erros);
        }

        protected virtual string[] AdicionarRulerset => null;
        protected virtual string[] AtualizarRulerset => null;
        protected abstract List<PropriedadeValor> ConfigurarAtualizacoes(TEntidade entidade);

        public virtual Task<IEnumerable<TEntidade>> ObterTodosAsync(IFiltro<TEntidade> filtro, IPaginacao paginacao = null)
        {
            var paginador = paginacao?.Como<Paginador>();
            return consultaRepositorio.ObterTodosAsync(filtro, paginador);
        }

        public virtual async Task<TEntidade> ObterAsync(string id)
        {
            ValidarCampoVazio(nameof(Entidade.Id), id);
            var resultado = await consultaRepositorio.ObterAsync(id);
            return resultado ?? throw new RecursoNaoEncontradoExcecao("Registro não encontrado");
        }

        public virtual async Task<TEntidade> AdicionarAsync(TEntidade entidade)
        {
            validador.Validar(entidade, AdicionarRulerset);
            await comandoRepositorio.AdicionarAsync(entidade);
            return entidade;
        }

        public virtual Task<bool> AtualizarAsync(TEntidade entidade)
        {
            validador.Validar(entidade, AtualizarRulerset);

            var atualizacoes = ConfigurarAtualizacoes(entidade);
            return comandoRepositorio.AtualizarUmAsync(t => t.Id == entidade.Id, atualizacoes);
        }

        public virtual Task<bool> RemoverAsync(TEntidade entidade)
        {
            ValidarCampoVazio(nameof(Entidade.Id), entidade.Id);
            return comandoRepositorio.AtivarDesativarUmAsync(entidade.Id, false);

            throw new Exception("Não foi possível deletar. Para mais informações contate o administrador");
        }

        private void ValidarCampoVazio(string nome, string valor)
        {
            validadorSimples
               .Add(ValidadoresSimples.CampoVazio(nome, valor))
               .Validar();
        }
    }
}
