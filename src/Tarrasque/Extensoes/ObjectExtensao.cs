using AutoMapper;
using Nebularium.Tarrasque.Funcoes;
using Nebularium.Tarrasque.Gestores;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class ObjectExtensao
    {
        public static T ValorPropriedade<T>(this object obj, string propriedade)
        {
            try
            {
                return (T)obj.GetType().GetProperty(propriedade).GetValue(obj, null);
            }
            catch
            {
                return default(T);
            }
        }
        public static void SetaValorPropriedade(this object obj, string propriedade, object valor)
        {
            obj.GetType().GetProperty(propriedade).SetValue(obj, valor);
        }
        public static PropertyInfo ObterPropertyInfo<TSource, TProperty>(this object obj, Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);
            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(String.Format("Expressão '{0}', não se refere a uma propriedade.", propertyLambda));
            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(String.Format("Expressão '{0}', não se refere a uma propriedade.", propertyLambda));
            if (type != propInfo.PropertyType && !type.GetTypeInfo().IsSubclassOf(propInfo.PropertyType))
                throw new ArgumentException(String.Format("Expressão '{0}' se refere a uma propriedade que não é do tipo {1}.", propertyLambda, type));
            return propInfo;
        }
        public static Type ObterTypeSemProxy(this object tipo)
        {
            if (tipo == null)
                return null;
            return TypeExtensao.ObterTypeSemProxy(tipo.GetType());
        }
        public static TypeInfo ObterTypeInfoSemProxy(this object tipo)
        {
            return TypeExtensao.ObterTypeInfoSemProxy(tipo.GetType());
        }
        public static bool NuloOuDefault<T>(this T obj)
        {
            return obj == null || EqualityComparer<T>.Default.Equals(obj, default);
        }
        public static T Como<T>(this object obj, bool naoMapear = false, bool permitirExcecao = false, T valorPadrao = default)
        {
            try
            {
                if (obj == null)
                    return valorPadrao;

                if (obj.GetType() == typeof(T))
                    return (T)obj;

                if (obj is Enum)
                {
                    if (typeof(T) == typeof(int) || typeof(T) == typeof(byte) || typeof(T) == typeof(Enum) || typeof(T) == typeof(long))
                        return (T)obj;

                    if (valorPadrao.Equals(default(T)))
                        return ConverteUtils.SempreConverteEnum<T>(obj);
                    return ConverteUtils.SempreConverteEnum(obj, valorPadrao);
                }

                if (!naoMapear)
                {
                    var r = GestorDependencia.Instancia.ObterInstancia<IMapper>().Map<T>(obj);
                    return r;
                }

                return (T)obj;
            }
            catch (AutoMapperMappingException)
            {
                return (T)obj;
            }
            catch (Exception)
            {
                if (permitirExcecao)
                    throw;
                if (valorPadrao?.Equals(default(T)) ?? true)
                    return ConverteUtils.SempreConverte<T>(obj);
                return ConverteUtils.SempreConverte(obj, valorPadrao);
            }
        }
    }
}
