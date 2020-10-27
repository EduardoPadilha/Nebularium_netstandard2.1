using Nebularium.Tiamat.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nebularium.Tiamat.Servicos
{
    public abstract class ConsultaServico<TEntidade> : IConsultaServico<TEntidade> where TEntidade : IEntidade, new()
    {
        private readonly IConsultaRepositorio<TEntidade> repositorioConsulta;

        public ConsultaServico(IConsultaRepositorio<TEntidade> repositorioConsulta)
        {
            this.repositorioConsulta = repositorioConsulta;
        }
        public Task<TEntidade> ObterAsync(string id)
        {
            return repositorioConsulta.ObterAsync(id);
        }

        public Task<IEnumerable<TEntidade>> ObterTodosAsync<T>(IFiltro<T> filtro)
        {
            return repositorioConsulta.ObterTodosAsync(filtro);
        }

        public Task<IEnumerable<TEntidade>> ObterTodosAsync<T>(Expression<Func<T, bool>> predicado)
        {
            return repositorioConsulta.ObterTodosAsync(predicado);
        }

        public Task<IEnumerable<TEntidade>> ObterTodosAsync<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> predicado)
        {
            return repositorioConsulta.ObterTodosAsync(predicado);
        }
    }
}
