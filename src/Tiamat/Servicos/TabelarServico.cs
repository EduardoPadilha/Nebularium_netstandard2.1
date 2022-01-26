using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Recursos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nebularium.Tiamat.Servicos
{
    public abstract class TabelarServico<TEntidade> : ITabelarServico<TEntidade>
    {
        private readonly ITabelarRepositorio<TEntidade> repositorio;

        protected TabelarServico(ITabelarRepositorio<TEntidade> repositorio)
        {
            this.repositorio = repositorio;
        }

        public virtual async Task<TEntidade> AdicionarAsync(TEntidade entidade)
        {
            await repositorio.AdicionarAsync(entidade);
            return entidade;
        }

        public virtual async Task<bool> AtualizarAsync(TEntidade entidade)
        {
            return await repositorio.AtualizarAsync(entidade);
        }

        public virtual async Task<TEntidade> ObterAsync(string chaveParticao, string chaveLinha)
        {
            return await repositorio.ObterAsync(chaveParticao, chaveLinha);
        }

        public virtual async Task<IEnumerable<TEntidade>> ObterTodosAsync(string chaveParticao, IPaginacao paginacao = null)
        {
            var paginador = paginacao?.Como<Paginador>();
            return await repositorio.ObterTodosAsync(chaveParticao, paginador);
        }

        public virtual async Task<bool> RemoverAsync(TEntidade entidade)
        {
            return await repositorio.RemoverAsync(entidade);
        }
    }
}
