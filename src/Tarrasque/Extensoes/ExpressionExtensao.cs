using Nebularium.Tarrasque.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class ExpressionExtensao
    {
        public static Expression<Func<T1, T2>> Or<T1, T2>(this Expression<Func<T2, T2>> left,
           Expression<Func<T1, T2>> right)
        {
            Expression<Func<T1, T2>> combined = Expression.Lambda<Func<T1, T2>>(
                Expression.OrElse(left.Body,
                new SubstituidorParametroExpression(right.Parameters, left.Parameters).Visit(right.Body)), left.Parameters);

            return combined;
        }

        public static Expression<Func<T1, T2>> And<T1, T2>(this Expression<Func<T1, T2>> left,
            Expression<Func<T1, T2>> right)
        {
            Expression<Func<T1, T2>> combined = Expression.Lambda<Func<T1, T2>>(
                Expression.AndAlso(left.Body,
                    new SubstituidorParametroExpression(right.Parameters, left.Parameters).Visit(right.Body)), left.Parameters);

            return combined;
        }
        /// <summary>
        /// Converte uma expression de um tipo para outro tipo mapeado no AutoMapper
        /// Depende do automapper instalado e devidamente configurado assim como entidades mapeadas
        /// </summary>
        /// <typeparam name="T">Tipo origem da conversão</typeparam>
        /// <typeparam name="TProxy">Tipo destino da conversão</typeparam>
        /// <param name="predicado">Expression a ser convertido</param>
        /// <returns></returns>
        public static Expression<Func<TProxy, bool>> ConvertePredicado<T, TProxy>(this Expression<Func<T, bool>> predicado)
        {
            return predicado.Como<Expression<Func<TProxy, bool>>>();
        }

        /// <summary>
        /// Converte uma expression de um tipo para outro tipo mapeado no AutoMapper
        /// Depende do automapper instalado e devidamente configurado assim como entidades mapeadas
        /// </summary>
        /// <typeparam name="T">Tipo origem da conversão</typeparam>
        /// <typeparam name="TProxy">Tipo destino da conversão</typeparam>
        /// /// <typeparam name="TProp">Tipo da propriedade</typeparam>
        /// <param name="predicado">Expression a ser convertido</param>
        /// <returns></returns>
        public static Expression<Func<TProxy, TProp>> ConvertePredicado<T, TProxy, TProp>(this Expression<Func<T, TProp>> predicado)
        {
            return predicado.Como<Expression<Func<TProxy, TProp>>>();
        }

        /// <summary>
        /// Converte uma expression de um tipo para outro tipo mapeado no AutoMapper
        /// Depende do automapper instalado e devidamente configurado assim como entidades mapeadas
        /// </summary>
        /// <typeparam name="T">Tipo origem da conversão</typeparam>
        /// <typeparam name="TProxy">Tipo destino da conversão</typeparam>
        /// <param name="predicado">Expression a ser convertido</param>
        /// <returns></returns>
        public static Expression<Func<IQueryable<TProxy>, IQueryable<TProxy>>> ConvertePredicado<T, TProxy>(this Expression<Func<IQueryable<T>, IQueryable<T>>> predicado)
        {
            return predicado.Como<Expression<Func<IQueryable<TProxy>, IQueryable<TProxy>>>>();
        }

        public static MemberExpression ObterMemberInfo(this Expression method)
        {
            var lambda = method as LambdaExpression;
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }
        public static PropertyInfo ObterPropertyInfo<TSource, TProperty>(this Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);
            MemberExpression member = propertyLambda.ObterMemberInfo();//propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format("Expressão '{0}', não se refere a uma propriedade.", propertyLambda));
            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format("Expressão '{0}', não se refere a uma propriedade.", propertyLambda));
            if (type != propInfo.PropertyType && !type.GetTypeInfo().IsSubclassOf(propInfo.PropertyType))
                throw new ArgumentException(string.Format("Expressão '{0}' se refere a uma propriedade que não é do tipo {1}.", propertyLambda, type));
            return propInfo;
        }

        //private static MemberExpression GetMemberExpression(Expression expression)
        //{
        //    MemberExpression memberExpression = null;
        //    if (expression.NodeType == ExpressionType.Convert)
        //    {
        //        var body = (UnaryExpression)expression;
        //        memberExpression = body.Operand as MemberExpression;
        //    }
        //    else if (expression.NodeType == ExpressionType.MemberAccess)
        //    {
        //        memberExpression = expression as MemberExpression;
        //    }

        //    return memberExpression;
        //}

        /// <summary>
		/// Gets the property from the expression.
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="expression">The expression.</param>
		/// <returns>The <see cref="PropertyInfo"/> for the expression.</returns>
		public static MemberInfo GetMember<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            //var member = GetMemberExpression(expression.Body).Member
            var member = expression.Body.ObterMemberInfo().Member;
            var property = member as PropertyInfo;
            if (property != null)
            {
                return property;
            }

            var field = member as FieldInfo;
            if (field != null)
            {
                return field;
            }

            throw new Exception($"'{member.Name}' is not a member.");
        }

        /// <summary>
        /// Gets the member inheritance chain as a stack.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The member expression.</param>
        /// <returns>The inheritance chain for the given member expression as a stack.</returns>
        public static Stack<MemberInfo> GetMembers<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            var stack = new Stack<MemberInfo>();

            var currentExpression = expression.Body;
            while (true)
            {
                //var memberExpression = GetMemberExpression(currentExpression);
                var memberExpression = currentExpression.ObterMemberInfo();
                if (memberExpression == null)
                {
                    break;
                }

                stack.Push(memberExpression.Member);
                currentExpression = memberExpression.Expression;
            }

            return stack;
        }
    }
}
