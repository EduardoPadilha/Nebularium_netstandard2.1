﻿using System.Threading.Tasks;

namespace Nebularium.Tiamat.Interfaces
{
    public interface IComandoServico<TEntidade> where TEntidade : IEntidade, new()
    {
        Task AdicionarAsync(TEntidade entidade);
        Task AtualizarAsync(TEntidade entidade);
        Task RemoverAsync(TEntidade entidade);
    }
}