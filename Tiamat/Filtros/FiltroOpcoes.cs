using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Nebularium.Tiamat.Filtros
{
    public class FiltroOpcoes<TEntidade>
    {
        private readonly Dictionary<string, Expression<Func<TEntidade, bool>>> condicoes;
        private readonly string regra;
        internal FiltroOpcoes(string regra, Dictionary<string, Expression<Func<TEntidade, bool>>> condicoes)
        {
            this.regra = regra;
            this.condicoes = condicoes;
        }
        public void sobCondicional(Expression<Func<TEntidade, bool>> condicao)
        {
            condicoes.Add(regra, condicao);
        }
    }
}
