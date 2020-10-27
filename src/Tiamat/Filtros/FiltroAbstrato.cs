﻿using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tiamat.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nebularium.Tiamat.Filtros
{
    public class FiltroAbstrato<TEntidade> : IFiltro<TEntidade> where TEntidade : IEntidade, new()
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
        public IFiltroOpcoes<TEntidade> AdicionarRegra(Expression<Func<TEntidade, bool>> criterio)
        {
            var id = Guid.NewGuid().ToString("N");
            var r = new FiltroOpcoes<TEntidade>(id, condicoes);
            criterios.Add(id, criterio);
            return r;
        }
        public void SetarModeloCriterio(TEntidade criterio)
        {
            Criterio = criterio;
        }
        public IQueryable<TEntidade> ObterFiltro(IQueryable<TEntidade> lista)
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
        public Expression<Func<TEntidade, bool>> ObterPredicados()
        {
            Expression<Func<TEntidade, bool>> ExpressoesAtivas = exp => true;
            foreach (var k in criterios.Keys)
            {
                if (condicoes.ContainsKey(k))
                    if (condicoes[k].Compile().Invoke(Criterio))
                        ExpressoesAtivas = ExpressoesAtivas.And(criterios[k]);
            }
            return ExpressoesAtivas;
        }
    }
}