using Microsoft.Extensions.Logging;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Comandos;
using Nebularium.Tiamat.Entidades;
using Nebularium.Tiamat.Extensoes;
using Nebularium.Tiamat.Recursos;
using Nebularium.Tiamat.Validacoes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Noctua.Dominio.Features
{
    public abstract class ComandosCrud<TEntidade> : ComandoBase<TEntidade>,
        IComandosCrud<TEntidade>
        where TEntidade : Entidade, new()
    {
        protected new readonly IComandoRepositorio<TEntidade> repositorio;
        protected ComandosCrud(IContextoNotificacao notificacao,
            IComandoRepositorio<TEntidade> repositorio,
            ILogger<TEntidade> logger) :
            base(notificacao, repositorio, logger)
        {
            this.repositorio = repositorio;
        }

        public Task AdicionarUmAsync(TEntidade entidade)
        {
            //Validar(nameof(ComandosCrud<TEntidade>.AdicionarUmAsync), entidade);
            return repositorio.AdicionarAsync(entidade);
        }

        public Task AdicionarMuitosAsync(IEnumerable<TEntidade> entidades)
        {
            //foreach (var entidade in entidades)
                //Validar(nameof(ComandosCrud<TEntidade>.AdicionarMuitosAsync), entidade);
                return repositorio.AdicionarAsync(entidades);
        }

        public Task<bool> AtualizarUmAsync(Expression<Func<TEntidade, bool>> predicado, List<PropriedadeValor> propriedades)
        {
            var entidade = propriedades.ParaEntidade<TEntidade>();
            //Validar(nameof(ComandosCrud<TEntidade>.AtualizarUmAsync), entidade);
            return repositorio.AtualizarUmAsync(predicado, propriedades);
        }

        public Task<bool> AtualizarMuitosAsync(Expression<Func<TEntidade, bool>> predicado, List<PropriedadeValor> propriedades)
        {
            var entidade = propriedades.ParaEntidade<TEntidade>();
            //Validar(nameof(ComandosCrud<TEntidade>.AtualizarMuitosAsync), entidade);
            return repositorio.AtualizarMuitosAsync(predicado, propriedades);
        }

        public Task<bool> RemoverUmAsync(string id)
        {
            validadorSimples.Add(ValidadoresSimples.Id(id));
            //Validar(nameof(ComandosCrud<TEntidade>.RemoverUmAsync), null);
            return repositorio.RemoverUmAsync(id);
        }

        public Task<bool> RemoverMuitosAsync(Expression<Func<TEntidade, bool>> predicado)
        {
            return repositorio.RemoverMuitosAsync(predicado);
        }

        public Task<bool> AtivarUmAsync(string id)
        {
            validadorSimples.Add(ValidadoresSimples.Id(id));
            //Validar(nameof(ComandosCrud<TEntidade>.AtivarUmAsync), null);
            return repositorio.AtivarDesativarUmAsync(id, true);
        }

        public Task<bool> DesativarUmAsync(string id)
        {
            validadorSimples.Add(ValidadoresSimples.Id(id));
            //Validar(nameof(ComandosCrud<TEntidade>.DesativarUmAsync), null);
            return repositorio.AtivarDesativarUmAsync(id, false);
        }
    }
}
