using Nebularium.Tiamat.Recursos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nebularium.Tiamat.Abstracoes
{
    public interface IComandoRepositorioBase<TEntidade> where TEntidade : IEntidade, new()
    {
        Task AdicionarAsync(TEntidade entidade);
        Task AdicionarAsync(IEnumerable<TEntidade> entidades);
        Task<bool> AtualizarUmAsync(Expression<Func<TEntidade, bool>> predicado, List<PropriedadeValor> propriedades);
        Task<bool> AtualizarMuitosAsync(Expression<Func<TEntidade, bool>> predicado, List<PropriedadeValor> propriedades);
        Task<bool> RemoverUmAsync(string id);
        Task<bool> RemoverMuitosAsync(Expression<Func<TEntidade, bool>> predicado);
    }
}
