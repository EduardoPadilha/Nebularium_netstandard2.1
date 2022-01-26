using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nebularium.Cthulhu.Enums;
using Nebularium.Cthulhu.Extensoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Entidades;
using Nebularium.Tiamat.Excecoes;
using Nebularium.Tiamat.Paginadores;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Nebularium.Cthulhu.Abstracoes
{
    public abstract class FuncaoCrudEntidade<TEntidade, TDto>
        where TEntidade : Entidade, new()
        where TDto : class, new()
    {
        protected readonly IServicoCrudEntidade<TEntidade> servico;

        public FuncaoCrudEntidade(IServicoCrudEntidade<TEntidade> servico)
        {
            this.servico = servico;
        }

        public virtual Task<IActionResult> EntradaCrudAsync(HttpRequest req, ILogger log, string id)
        {
            var verbo = Enum.Parse<VerboHTTP>(req.Method);
            switch (verbo)
            {
                case VerboHTTP.GET:
                    var todos = id.limpoNuloBrancoOuZero();
                    return todos ? ObterTodosAsync(req, log) : ObterPorIdAsync(log, id);
                case VerboHTTP.POST:
                    return AdicionarAsync(req, log);
                case VerboHTTP.PUT:
                    return AtualizarAsync(req, log, id);
                case VerboHTTP.DELETE:
                    return DeletarAsync(log, id);
                default:
                    return Task.FromResult(RetornaFalha(log, "O recurso que você procura não existe"));
            }
        }

        protected abstract IFiltro<TEntidade> CriarFiltro(HttpRequest req);
        protected IPaginacao CriarPaginacao(HttpRequest req)
        {
            var pagina = req.Query.Obter<int>("pagina");
            var tamanhoPagina = req.Query.Obter<int>("tamanho");

            if (tamanhoPagina.NuloOuDefault() || pagina.NuloOuDefault()) return null;

            return new Paginacao { Pagina = pagina, TamanhoPagina = tamanhoPagina };
        }

        public virtual async Task<IActionResult> ObterTodosAsync(HttpRequest req, ILogger log)
        {
            try
            {
                var filtro = CriarFiltro(req);
                var paginacao = CriarPaginacao(req);
                var resultado = await servico.ObterTodosAsync(filtro, paginacao);
                return RetornarSucesso<IEnumerable<TDto>>(resultado);
            }
            catch (ValidacaoExcecao e)
            {
                return RetornaFalha(log, JsonConvert.SerializeObject(e.Erros));
            }
            catch (Exception e)
            {
                return RetornaFalha(log, e.GetBaseException().Message);
            }
        }

        public virtual async Task<IActionResult> ObterPorIdAsync(ILogger log, string id)
        {
            try
            {
                var resultado = await servico.ObterAsync(id); ;
                return RetornarSucesso<TDto>(resultado);
            }
            catch (ValidacaoExcecao e)
            {
                return RetornaFalha(log, JsonConvert.SerializeObject(e.Erros));
            }
            catch (RecursoNaoEncontradoExcecao ex)
            {
                return new NotFoundObjectResult(ex.Message);
            }
            catch (Exception e)
            {
                return RetornaFalha(log, e.GetBaseException().Message);
            }
        }

        public async Task<IActionResult> AdicionarAsync(HttpRequest req, ILogger log)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            try
            {
                var resultado = await SalvarAsync(content, true);
                return RetornarSucesso<TDto>(resultado);
            }
            catch (ValidacaoExcecao e)
            {
                return RetornaFalha(log, JsonConvert.SerializeObject(e.Erros));
            }
            catch (Exception e)
            {
                return RetornaFalha(log, e.GetBaseException().Message);
            }
        }

        public async Task<IActionResult> AtualizarAsync(HttpRequest req, ILogger log, string id)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            try
            {
                var resultado = await SalvarAsync(content, false, id);
                return RetornarSucesso<TDto>(resultado);
            }
            catch (ValidacaoExcecao e)
            {
                return RetornaFalha(log, JsonConvert.SerializeObject(e.Erros));
            }
            catch (Exception e)
            {
                return RetornaFalha(log, e.GetBaseException().Message);
            }
        }

        public virtual async Task<IActionResult> DeletarAsync(ILogger log, string id)
        {
            try
            {
                var entidade = Activator.CreateInstance<TEntidade>();
                entidade.Id = id;
                await servico.RemoverAsync(entidade);
                return new OkResult();
            }
            catch (ValidacaoExcecao e)
            {
                return RetornaFalha(log, JsonConvert.SerializeObject(e.Erros));
            }
            catch (Exception e)
            {
                return RetornaFalha(log, e.GetBaseException().Message);
            }
        }

        public virtual async Task<IActionResult> AtivarDesativarAsync(ILogger log, string id, bool ativar)
        {
            try
            {
                var entidade = Activator.CreateInstance<TEntidade>();
                entidade.Id = id;
                await servico.AtivarDesativarAsync(entidade, ativar);
                return new OkResult();
            }
            catch (ValidacaoExcecao e)
            {
                return RetornaFalha(log, JsonConvert.SerializeObject(e.Erros));
            }
            catch (Exception e)
            {
                return RetornaFalha(log, e.GetBaseException().Message);
            }
        }

        protected virtual async Task<TEntidade> SalvarAsync(string body, bool novo, string id = null)
        {
            var dto = JsonConvert.DeserializeObject<TDto>(body);
            var entidade = dto.Como<TEntidade>();
            if (novo)
                return await servico.AdicionarAsync(entidade);

            entidade.Id = id;
            var resultado = await servico.AtualizarAsync(entidade);
            if (resultado) return entidade;

            throw new Exception("Não foi possível atualizar o registro, contate o administrador");
        }

        protected virtual IActionResult RetornaFalha(ILogger log, string mensagem)
        {
            log.LogError(mensagem);
            return new BadRequestObjectResult(mensagem);
        }

        protected virtual IActionResult RetornarSucesso<T>(object resultado)
        {
            return new OkObjectResult(resultado.Como<T>());
        }


    }
}
