﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nebularium.Cthulhu.Enums;
using Nebularium.Cthulhu.Extensoes;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Excecoes;
using Nebularium.Tiamat.Paginadores;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Nebularium.Cthulhu.Abstracoes
{
    public abstract class FuncaoCrudTabelar<TEntidade, TDto>
        where TEntidade : new()
        where TDto : class, new()
    {
        protected readonly ITabelarServico<TEntidade> servico;

        public FuncaoCrudTabelar(ITabelarServico<TEntidade> servico)
        {
            this.servico = servico;
        }

        #region Abstratos
        protected abstract TEntidade PreencheChaveParticao(TEntidade entidade, string chaveParticao);
        protected abstract TEntidade PreencheChaveLinha(TEntidade entidade, string chaveLinha);
        #endregion

        public virtual Task<IActionResult> EntradaCrudAsync(HttpRequest req, ILogger log, string chaveParticao, string chaveLinha)
        {
            var verbo = Enum.Parse<VerboHTTP>(req.Method);
            switch (verbo)
            {
                case VerboHTTP.GET:
                    var todos = chaveLinha.limpoNuloBrancoOuZero();
                    return todos ? ObterTodosAsync(req, log, chaveParticao) : ObterPorIdAsync(log, chaveParticao, chaveLinha);
                case VerboHTTP.POST:
                    return AdicionarAsync(req, log, chaveParticao);
                case VerboHTTP.PUT:
                    return AtualizarAsync(req, log, chaveParticao, chaveLinha);
                case VerboHTTP.DELETE:
                    return DeletarAsync(log, chaveParticao, chaveLinha);
                default:
                    return Task.FromResult(RetornaFalha(log, "O recurso que você procura não existe"));
            }
        }

        public virtual async Task<IActionResult> ObterTodosAsync(HttpRequest req, ILogger log, string chaveParticao)
        {
            try
            {
                var paginacao = CriarPaginacao(req);
                var resultado = await servico.ObterTodosAsync(chaveParticao, paginacao);
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

        public virtual async Task<IActionResult> ObterPorIdAsync(ILogger log, string chaveParticao, string chaveLinha)
        {
            try
            {
                var resultado = await servico.ObterAsync(chaveParticao, chaveLinha);
                if (resultado == null)
                    return RetornaFalha(log, "Registro não encontrado");

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

        public virtual async Task<IActionResult> AdicionarAsync(HttpRequest req, ILogger log, string chaveParticao)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            try
            {
                var resultado = await SalvarAsync(content, chaveParticao);
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

        public virtual async Task<IActionResult> AtualizarAsync(HttpRequest req, ILogger log, string chaveParticao, string chaveLinha)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            try
            {
                var resultado = await SalvarAsync(content, chaveParticao, chaveLinha);
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

        public virtual async Task<IActionResult> DeletarAsync(ILogger log, string chaveParticao, string chaveLinha)
        {
            try
            {
                var entidade = Activator.CreateInstance<TEntidade>();
                entidade = PreencheChaves(entidade, chaveParticao, chaveLinha);
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

        protected virtual async Task<TEntidade> SalvarAsync(string body, string chaveParticao, string chaveLinha = null)
        {
            var dto = JsonConvert.DeserializeObject<TDto>(body);
            var entidade = dto.Como<TEntidade>();
            entidade = PreencheChaveParticao(entidade, chaveParticao);
            if (chaveLinha.LimpoNuloBranco())
                return await servico.AdicionarAsync(entidade);

            entidade = PreencheChaveLinha(entidade, chaveLinha);
            var resultado = await servico.AtualizarAsync(entidade);
            if (resultado) return entidade;

            throw new Exception("Não foi possível atualizar o registro, contate o administrador");
        }

        #region Metodos de suporte
        protected IPaginacao CriarPaginacao(HttpRequest req)
        {
            var pagina = req.Query.Obter<int>("pagina");
            var tamanhoPagina = req.Query.Obter<int>("tamanho");

            if (tamanhoPagina.NuloOuDefault() || pagina.NuloOuDefault()) return null;

            return new Paginacao { Pagina = pagina, TamanhoPagina = tamanhoPagina };
        }

        private TEntidade PreencheChaves(TEntidade entidade, string chaveParticao, string chaveLinha)
        {
            PreencheChaveParticao(entidade, chaveParticao);
            PreencheChaveLinha(entidade, chaveLinha);
            return entidade;
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

        #endregion


    }
}
