using Nebularium.Tiamat.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nebularium.Tiamat.Filtros
{
    public class FiltroAbstrato<TEntidade> where TEntidade : IEntidade, new()
    {
        private readonly Dictionary<string, Expression<Func<TEntidade, bool>>> criterios;
        private readonly Dictionary<string, Expression<Func<TEntidade, bool>>> condicoes;
        public TEntidade Criterio { get; set; }
        protected FiltroAbstrato()
        {
            Criterio = new TEntidade();
            criterios = new Dictionary<string, Expression<Func<TEntidade, bool>>>();
            condicoes = new Dictionary<string, Expression<Func<TEntidade, bool>>>();
        }
        public FiltroOpcoes<TEntidade> adicionarRegraSimples(Expression<Func<TEntidade, bool>> criterio)
        {
            return adicionarRegraComposta(Guid.NewGuid().ToString(), criterio);
        }
        public FiltroOpcoes<TEntidade> adicionarRegraComposta(string regra, Expression<Func<TEntidade, bool>> criterio)
        {
            var r = new FiltroOpcoes<TEntidade>(regra, condicoes);
            criterios.Add(regra, criterio);
            return r;
        }
        public void setarModeloCriterio(TEntidade criterio)
        {
            Criterio = criterio;
        }
        public IQueryable<TEntidade> obterFiltro(IQueryable<TEntidade> lista)
        {
            foreach (var k in criterios.Keys)
            {
                if (condicoes.ContainsKey(k))
                {
                    if (condicoes[k].Compile().Invoke(Criterio))
                        lista = lista.Where(criterios[k]);
                }
                else
                    lista = lista.Where(criterios[k]);
            }
            return lista;
        }
    }
}
