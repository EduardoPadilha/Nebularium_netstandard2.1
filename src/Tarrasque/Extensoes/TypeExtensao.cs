using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class TypeExtensao
    {
        private static readonly Type[] tiposPrimitivos = { typeof(DateTime), typeof(String), typeof(Decimal) };

        #region Anotações
        public static TAtributo ObterAnotacao<TAtributo>(this Type tipo) where TAtributo : Attribute
        {
            return tipo.GetCustomAttributes(typeof(TAtributo), true).FirstOrDefault() as TAtributo;
        }
        public static TAtributo ObterAnotacao<TAtributo, TRetorno>(this Type tipo, string metodo) where TAtributo : Attribute
        {
            return tipo.GetMethods().FirstOrDefault(a => a.Name == metodo && a.ReturnType == typeof(TRetorno))
                                    ?.GetCustomAttributes<TAtributo>(true)?.FirstOrDefault();
        }
        public static TAtributo ObterAnotacao<TAtributo>(this Type tipo, string metodo) where TAtributo : Attribute
        {
            return tipo.GetMethod(metodo)?.GetCustomAttributes<TAtributo>(true)?.FirstOrDefault();
        }
        public static bool TemAnotacao<TAtributo>(this Type tipo) where TAtributo : Attribute
        {
            return tipo.GetCustomAttributes(typeof(TAtributo), true).Any();
        }
        public static bool TemAnotacao<TAtributo>(this Type tipo, string metodo) where TAtributo : Attribute
        {
            return tipo.GetMethod(metodo)?.GetCustomAttributes<TAtributo>(true)?.Any() ?? false;
        }
        public static bool TemAnotacao<TAtributo, TRetorno>(this Type tipo, string metodo) where TAtributo : Attribute
        {
            return tipo.GetMethods().FirstOrDefault(a => a.Name == metodo && a.ReturnType == typeof(TRetorno))
                                    ?.GetCustomAttributes<TAtributo>(true)?.Any() ?? false;
        }
        public static object ObterValorAnotacao<TAtributo>(this Type tipo, string propriedade) where TAtributo : Attribute
        {
            return tipo.ObterAnotacao<TAtributo>()?.GetType().GetProperty(propriedade).GetValue(tipo.ObterAnotacao<TAtributo>(), null);
        }
        public static object ObterValorAnotacao<TAtributo, TModel>(this Type tipo, Expression<Func<TModel, object>> propriedade) where TAtributo : Attribute
        {
            if (propriedade.Body.NodeType == ExpressionType.MemberAccess)
                return tipo.ObterAnotacao<TAtributo>().GetType().GetProperty(((MemberExpression)propriedade.Body).Member.Name)
                                                                .GetValue(tipo.ObterAnotacao<TAtributo>(), null);
            return null;
        }
        public static List<PropertyInfo> ObterPropriedadesAnotadas<TAtributo>(this Type tipo) where TAtributo : Attribute
        {
            return tipo.GetProperties().Where(prop => prop.GetCustomAttributes<TAtributo>(true).Any()).ToList();
        }
        #endregion

        /// <summary>
        /// Retorna se o tipo implementa a interface passada como parametro
        /// </summary>
        /// <typeparam name="TInterface">Interface que sera testada se o tipo implementa</typeparam>
        /// <param name="tipo">Tipo que sera testado</param>
        /// <returns>Retorna true se a interface for implementada pelo titulo ou false, caso não seja</returns>
        public static bool ImplementaInterface<TInterface>(this Type tipo)
        {
            return tipo.GetInterfaces().Any(i => i.FullName == typeof(TInterface).FullName);
        }
        /// <summary>
        /// Testa se o Type é um tipo primitivo nullable ex: (int?)
        /// </summary>
        public static bool IsPrimitivoNullable(this Type tipo)
        {
            var typeInfo = tipo.GetTypeInfo();
            return typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>) && tipo.GetGenericArguments().Any(t => t.GetTypeInfo().IsValueType && t.GetTypeInfo().IsPrimitive);
        }
        /// <summary>
        /// Testa se o Type é um tipo basico nullable ex: (int?) ou se é um DateTime? Decimal?
        /// </summary>
        public static bool IsBasicoNullable(this Type tipo)
        {
            var typeInfo = tipo.GetTypeInfo();
            return typeInfo.IsGenericType && tipo.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                   tipo.GetGenericArguments().Any(t => t.GetTypeInfo().IsValueType && (t.GetTypeInfo().IsPrimitive || tiposPrimitivos.Contains(t)));
        }
        /// <summary>
        /// Testa se o Type é um tipo primitivo ex: (int, bool, etc..) ou se é um DateTime, Decimal ou String
        /// </summary>
        public static bool IsBasico(this Type tipo)
        {
            var typeInfo = tipo.GetTypeInfo();
            return typeInfo.IsPrimitive || tiposPrimitivos.Contains(tipo);
        }
        /// <summary>
        /// Testa primeiro se o Type é um tipo primitivo ou DateTime, Decimal ou String
        /// Caso contrário testa se é um primitivo nullable ou se é um DateTime? Decimal?
        /// </summary>
        public static bool IsBasicoOuNullable(this Type tipo)
        {
            return IsBasico(tipo) || IsBasicoNullable(tipo);
        }
        public static bool IsColecao(this Type tipo)
        {
            return typeof(IList).IsAssignableFrom(tipo);
        }
        public static bool IsNullable(this Type type)
        {
            if (type.GetTypeInfo().IsGenericType)
                return type.GetGenericTypeDefinition() == typeof(Nullable<>);
            return false;
        }
        public static Type ObterTipoDoNullable(this Type type)
        {
            return type.GetTypeInfo().GenericTypeArguments[0];
        }
        public static Type PrimeiroTipoGenerico(this Type tipo)
        {
            return tipo.GetTypeInfo().IsGenericType ? tipo.GetGenericArguments()[0] : null;
        }
        public static object ConstruirNovo(this Type tipo)
        {
            var construtores = tipo.GetConstructors();
            var construtor = construtores.FirstOrDefault(c => c.GetParameters().Length == 0);
            if (construtor == null)
                throw new NotSupportedException();
            var obj = construtor.Invoke(null);
            return obj;
        }
        public static TypeInfo ObterTypeInfoSemProxy(this Type tipo)
        {
            var type = tipo.GetTypeInfo();
            if (type.IsClass && type.Namespace.Equals("System.Data.Entity.DynamicProxies"))
                type = type.BaseType.GetTypeInfo();
            return type;
        }
        public static Type ObterTypeSemProxy(this Type tipo)
        {
            if (tipo.GetTypeInfo().IsClass && tipo.Namespace.Equals("System.Data.Entity.DynamicProxies"))
                tipo = tipo.GetTypeInfo().BaseType;
            return tipo;
        }
        public static PropertyInfo ObterPropriedadeMaisBasica(this Type type, string name)
        {
            while (type != null)
            {
                var property = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                if (property != null)
                {
                    return property;
                }
                type = type.GetTypeInfo().BaseType;
            }
            return null;
        }
    }
}
