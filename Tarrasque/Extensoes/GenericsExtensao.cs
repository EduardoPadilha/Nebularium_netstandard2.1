using AutoMapper;
using Nebularium.Tarrasque.Configuracoes;
using Nebularium.Tarrasque.Gestores;
using System;
using System.Linq.Expressions;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class GenericsExtensao
    {
        //[DebuggerNonUserCode]
        public static T Injete<T, D>(this T obj, D complemento)
        {
            return GestorDependencia.Instancia.ObterInstancia<IMapper>().Map(complemento, obj);
        }
        public static String ObterNomePropriedade<TModel, TResult>(this Expression<Func<TModel, TResult>> prop)
        {
            if (prop == null)
                return "";
            MemberExpression member;
            switch (prop.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = prop.Body as UnaryExpression;
                    member = ue?.Operand as MemberExpression;
                    break;
                default:
                    member = prop.Body as MemberExpression;
                    break;
            }
            return member?.Member.Name ?? String.Empty;
        }
        public static string ObterDisplay<TModel, TResult>(this TModel obj, Expression<Func<TModel, TResult>> prop)
        {
            return Configuracao.DisplayNameExtrator().ObterDisplay(obj, prop.ObterNomePropriedade());
        }
    }
}
