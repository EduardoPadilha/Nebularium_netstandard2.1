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
        public Task AdicionarAsync(TEntidade entidade)
        {
            return repositorioComando.AdicionarAsync(entidade);
        }

        public Task AtualizarAsync(TEntidade entidade)
        {
            return repositorioComando.AtualizarAsync(entidade);
        }

        public Task RemoverAsync(TEntidade entidade)
        {
            return repositorioComando.RemoverAsync(entidade);
        }
    }
}
