using Nebularium.Tiamat.Interfaces;
using System.Threading.Tasks;

namespace Nebularium.Tiamat.Servicos
{
    public abstract class ComandoServico<TEntidade> : IComandoServico<TEntidade> where TEntidade : IEntidade, new()
    {
        private readonly IComandoRepositorio<TEntidade> repositorioComando;

        public ComandoServico(IComandoRepositorio<TEntidade> repositorioComando)
        {
            this.repositorioComando = repositorioComando;
        }
        public Task<TEntidade> Adicionar(TEntidade entidade)
        {
            return repositorioComando.Adicionar(entidade);
        }

        public Task Atualizar(TEntidade entidade)
        {
            return repositorioComando.Atualizar(entidade);
        }

        public Task Remover(TEntidade entidade)
        {
            return repositorioComando.Remover(entidade);
        }
    }
}
