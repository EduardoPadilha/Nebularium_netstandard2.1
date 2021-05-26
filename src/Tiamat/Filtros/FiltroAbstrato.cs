using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Abstracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nebularium.Tiamat.Filtros
{
    public abstract class FiltroAbstrato<TEntidade> : IFiltro<TEntidade> where TEntidade : class, new()
    {
        protected readonly Dictionary<string, Expression<Func<TEntidade, bool>>> criterios;
        protected readonly Dictionary<string, Expression<Func<TEntidade, bool>>> condicoes;
        public TEntidade Criterio { get; set; }
        protected FiltroAbstrato()
        {
            Criterio = new TEntidade();
            criterios = new Dictionary<string, Expression<Func<TEntidade, bool>>>();
            condicoes = new Dictionary<string, Expression<Func<TEntidade, bool>>>();
        }
        public virtual IFiltroOpcoes<TEntidade> AdicionarRegra(Expression<Func<TEntidade, bool>> criterio)
        {
            var id = Guid.NewGuid().ToString("N");
            var r = new FiltroOpcoes<TEntidade>(id, condicoes);
            criterios.Add(id, criterio);
            return r;
        }
        public virtual void SetarModeloCriterio(TEntidade criterio)
        {
            Criterio = criterio;
        }
        public virtual IQueryable<TEntidade> ObterFiltro(IQueryable<TEntidade> lista)
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
        public virtual Expression<Func<TEntidade, bool>> ObterPredicados()
        {
            Expression<Func<TEntidade, bool>> ExpressoesAtivas = exp => true;
            foreach (var k in criterios.Keys)
            {
                if (condicoes.ContainsKey(k))
                {
                    if (condicoes[k].Compile().Invoke(Criterio))
                        ExpressoesAtivas = ExpressoesAtivas.And(criterios[k]);
                }
                else
                    ExpressoesAtivas = ExpressoesAtivas.And(criterios[k]);
            }
            return ExpressoesAtivas;
        }
    }
    public abstract class FiltroAbstrato<TEntidade, TCriterio> : FiltroAbstrato<TEntidade> where TEntidade : class, new() where TCriterio : new()
    {
        public new TCriterio Criterio { get; set; }
        public FiltroAbstrato()
        {
            Criterio = new TCriterio();
        }
    }
}
