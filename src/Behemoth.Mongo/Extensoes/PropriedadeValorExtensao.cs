using AutoMapper;
using MongoDB.Driver;
using Nebularium.Tarrasque.Extensoes;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tiamat.Recursos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Nebularium.Behemoth.Mongo.Extensoes
{
    public static class PropriedadeValorExtensao
    {
        public static UpdateDefinition<TProxy> ObterUpdate<TProxy>(this List<PropriedadeValor> lista)
        {
            var mapper = GestorDependencia.Instancia.ObterInstancia<IMapper>();
            var builder = Builders<TProxy>.Update;
            List<UpdateDefinition<TProxy>> definicoes = new List<UpdateDefinition<TProxy>>();
            foreach (var propValor in lista)
            {
                var propValorTipo = propValor.Propriedade.Body.Type;
                propValorTipo = propValorTipo.IsColecao() ? propValorTipo.ObterTipoElemento() : propValorTipo;
                var convertido = propValor.Propriedade.Como<Expression<Func<TProxy, object>>>();
                if (!propValorTipo.IsBasico() && !propValorTipo.IsEnum)
                {
                    var valorConvertido = mapper.Map(propValor.Valor, propValor.Valor.GetType(), convertido.ObterMemberInfo().Type);
                    definicoes.Add(builder.Set(convertido, valorConvertido));
                }
                else
                    definicoes.Add(builder.Set(convertido, propValor.Valor));
            }

            return builder.Combine(definicoes);
        }

        //public static UpdateDefinition<TProxy> ObterUpdate2<TEntidade, TProxy>(this ListaPropriedadeValor<TEntidade> lista)
        //{
        //    var mapper = GestorDependencia.Instancia.ObterInstancia<IMapper>();
        //    var builder = Builders<TProxy>.Update;
        //    List<UpdateDefinition<TProxy>> definicoes = new List<UpdateDefinition<TProxy>>();
        //    foreach (var propValor in lista.ObterTodos)
        //    {
        //        var propValorTipo = propValor.Propriedade.Body.Type;
        //        propValorTipo = propValorTipo.IsColecao() ? propValorTipo.ObterTipoElemento() : propValorTipo;
        //        var convertido = mapper.Map(propValor.Propriedade, ReplaceNodes<TEntidade, TProxy>(propValor.Propriedade));//.Como<Expression<Func<TProxy, object>>>();
        //        //var convertido = propValor.Propriedade.Como<Expression<Func<TProxy, object>>>();
        //        if (!propValorTipo.IsBasico() && !propValorTipo.IsEnum)
        //        {
        //            var valorConvertido = mapper.Map(propValor.Valor, propValor.Valor.GetType(), ((LambdaExpression)convertido).Body.Type);
        //            definicoes.Add(builder.Set(convertido, valorConvertido));
        //        }
        //        else
        //            definicoes.Add(builder.Set(convertido, propValor.Valor));
        //    }

        //    return builder.Combine(definicoes);
        //}
        //private static Expression ReplaceNodes<TEntidade, TProxy>(Expression original)
        //{
        //    if (original.NodeType == ExpressionType.Lambda)
        //    {
        //        var lambdaExpression = (LambdaExpression)original;
        //        if (lambdaExpression.Parameters.Any(p => p.Type == typeof(TEntidade)))
        //        {
        //            var parameter = Expression.Parameter(typeof(TProxy), lambdaExpression.Parameters[0].Name);
        //            return Expression.Lambda(lambdaExpression.Body, parameter);
        //        }
        //    }
        //    return original;
        //}
    }

    //public class ParameterTypeVisitor<TSource, TTarget> : ExpressionVisitor
    //{
    //    private ReadOnlyCollection<ParameterExpression> _parameters;

    //    protected override Expression VisitParameter(ParameterExpression node)
    //    {
    //        return _parameters?.FirstOrDefault(p => p.Name == node.Name) ??
    //            (node.Type == typeof(TSource) ? Expression.Parameter(typeof(TTarget), node.Name) : node);
    //    }

    //    protected override Expression VisitLambda<T>(Expression<T> node)
    //    {
    //        _parameters = VisitAndConvert<ParameterExpression>(node.Parameters, "VisitLambda");
    //        return Expression.Lambda(Visit(node.Body), _parameters);
    //    }

    //    protected override Expression VisitMember(MemberExpression node)
    //    {
    //        if (node.Member.DeclaringType == typeof(TSource))
    //        {
    //            return Expression.Property(Visit(node.Expression), node.Member.Name);
    //        }
    //        return base.VisitMember(node);
    //    }
    //}
}
