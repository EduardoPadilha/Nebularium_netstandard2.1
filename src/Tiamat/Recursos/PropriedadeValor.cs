using Nebularium.Tarrasque.Extensoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Nebularium.Tiamat.Recursos
{
    public class PropriedadeValor
    {
        public PropriedadeValor(LambdaExpression propriedade, object valor, PropertyInfo info)
        {
            Propriedade = propriedade;
            Valor = valor;
            Info = info;
            Nome = info.Name;
        }
        public static PropriedadeValor Cria<TEntidade, TPropriedade>(Expression<Func<TEntidade, TPropriedade>> propriedade, TPropriedade valor)
        {
            var member = propriedade.ObterMemberInfo();
            var info = member.Member as PropertyInfo;
            return new PropriedadeValor(propriedade, valor, info);
        }
        public string Nome { get; private set; }
        public PropertyInfo Info { get; private set; }
        public LambdaExpression Propriedade { get; private set; }
        public object Valor { get; private set; }
    }

    public class PropriedadeValorFabrica<TEntidade>
    {
        private readonly List<PropriedadeValor> lista;
        public PropriedadeValorFabrica()
        {
            lista = new List<PropriedadeValor>();
        }

        public static PropriedadeValorFabrica<TEntidade> Iniciar()
        {
            return new PropriedadeValorFabrica<TEntidade>();
        }

        public List<PropriedadeValor> ObterTodos => lista;

        private bool Existe<TPropriedade>(Expression<Func<TEntidade, TPropriedade>> propriedade)
        {
            return lista.Select(pv => pv.Propriedade).Contains(propriedade);
        }

        public PropriedadeValorFabrica<TEntidade> Add<TPropriedade>(Expression<Func<TEntidade, TPropriedade>> propriedade, TPropriedade valor, bool ignorarNullOuDefault = true)
        {
            if (ignorarNullOuDefault && (valor == null || valor.Equals(default(TPropriedade)))) return this;
            if (Existe(propriedade)) return this;
            var propValor = PropriedadeValor.Cria(propriedade, valor);
            lista.Add(propValor);
            return this;
        }

        public PropriedadeValorFabrica<TEntidade> Remove<TPropriedade>(Expression<Func<TEntidade, TPropriedade>> propriedade)
        {
            lista.RemoveAll(pv => pv.Propriedade == propriedade);
            return this;
        }

        //public TPropriedade ObterValor<TPropriedade>(Expression<Func<TEntidade, TPropriedade>> propriedade)
        //{
        //    if (!Existe(propriedade)) return default(TPropriedade);
        //    return (TPropriedade)lista.FirstOrDefault(pv => pv.Propriedade == propriedade).Valor;
        //}

        //public (Expression<Func<TEntidade, TPropriedade>> propriedade, TPropriedade valor) Obter<TPropriedade>(Expression<Func<TEntidade, TPropriedade>> propriedade)
        //{
        //    if (!Existe(propriedade)) return (null, default);
        //    var propValor = lista.FirstOrDefault(pv => pv.Propriedade == propriedade);
        //    return ((Expression<Func<TEntidade, TPropriedade>>)propValor.Propriedade, (TPropriedade)propValor.Valor);
        //}
    }
}
