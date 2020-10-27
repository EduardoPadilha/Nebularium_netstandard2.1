using Nebularium.Tarrasque.Recursos;
using System;
using System.Linq.Expressions;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class ExpressionExtensao
    {
        /// <summary>
        /// Creates a lambda expression that represents a conditional OR operation
        /// </summary>
        /// <param name="left">An expression to set the left property of the binary expression</param>
        /// <param name="right">An expression to set the right property of the binary expression</param>
        /// <returns>A binary expression that has the node type property equal to OrElse, 
        /// and the left and right properties set to the specified values</returns>
        public static Expression<Func<T1, T2>> Or<T1, T2>(this Expression<Func<T2, T2>> left,
            Expression<Func<T1, T2>> right)
        {
            Expression<Func<T1, T2>> combined = Expression.Lambda<Func<T1, T2>>(
                Expression.OrElse(left.Body,
                new SubstituidorParametroExpression(right.Parameters, left.Parameters).Visit(right.Body)), left.Parameters);

            return combined;
        }

        /// <summary>
        /// Creates a lambda expression that represents a conditional AND operation
        /// </summary>
        /// <param name="left">An expression to set the left property of the binary expression</param>
        /// <param name="right">An expression to set the right property of the binary expression</param>
        /// <returns>A binary expression that has the node type property equal to AndAlso, 
        /// and the left and right properties set to the specified values</returns>
        public static Expression<Func<T1, T2>> And<T1, T2>(this Expression<Func<T1, T2>> left,
            Expression<Func<T1, T2>> right)
        {
            Expression<Func<T1, T2>> combined = Expression.Lambda<Func<T1, T2>>(
                Expression.AndAlso(left.Body,
                    new SubstituidorParametroExpression(right.Parameters, left.Parameters).Visit(right.Body)), left.Parameters);

            return combined;
        }
    }
}
