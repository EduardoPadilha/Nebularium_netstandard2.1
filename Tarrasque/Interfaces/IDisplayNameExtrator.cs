using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Nebularium.Tarrasque.Interfaces
{
    public interface IDisplayNameExtrator
    {
        string ObterDisplay(object obj, string propNome);
        string ObterDisplay(Type tipo, string propNome);
        string ObterDisplay(object obj, MemberInfo membro);
        string ObterDisplay(Type tipo, MemberInfo membro);
        string ObterDisplay<TSource, TProperty>(TSource tipo, Expression<Func<TSource, TProperty>> prop);
    }
}
