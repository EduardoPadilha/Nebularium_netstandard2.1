using Nebularium.Tiamat.Interfaces;
using System;
using System.Threading.Tasks;

namespace Nebularium.Behemoth.Mongo
{
    public class ComandoRepositorio<TEntidade, TProxy> : IComandoRepositorio<TEntidade>
        where TEntidade : IEntidade, new()
        where TProxy : IEntidade, new()
    {
        public Task<TEntidade> AdicionarAsync(TEntidade entidade)
        {
            throw new NotImplementedException();
        }

        public Task AtualizarAsync(TEntidade entidade)
        {
            throw new NotImplementedException();
        }

        public Task RemoverAsync(TEntidade entidade)
        {
            throw new NotImplementedException();
        }
    }
}
