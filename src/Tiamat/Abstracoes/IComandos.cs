using Nebularium.Tiamat.Recursos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nebularium.Tiamat.Abstracoes
{
    public interface IComandosCrud<TEntidade> : IAdicionarComando<TEntidade>,
        IAtualizarComando<TEntidade>,
        IRemoverComando<TEntidade>,
        IAtivarComando<TEntidade>,
        IDesativarComando<TEntidade> where TEntidade : IEntidade
    { }

    public interface IAdicionarComando<TEntidade> where TEntidade : IEntidade
    {
        Task AdicionarUmAsync(TEntidade entidade);
        Task AdicionarMuitosAsync(IEnumerable<TEntidade> entidades);
    }

    public interface IAtualizarComando<TEntidade> where TEntidade : IEntidade
    {
        Task<bool> AtualizarUmAsync(Expression<Func<TEntidade, bool>> predicado, List<PropriedadeValor> propriedades);
        Task<bool> AtualizarMuitosAsync(Expression<Func<TEntidade, bool>> predicado, List<PropriedadeValor> propriedades);
    }

    public interface IRemoverComando<TEntidade> where TEntidade : IEntidade
    {
        Task<bool> RemoverUmAsync(string id);
        Task<bool> RemoverMuitosAsync(Expression<Func<TEntidade, bool>> predicado);
    }

    public interface IAtivarComando<TEntidade> where TEntidade : IEntidade
    {
        Task<bool> AtivarUmAsync(string id);
    }

    public interface IDesativarComando<TEntidade> where TEntidade : IEntidade
    {
        Task<bool> DesativarUmAsync(string id);
    }

}
