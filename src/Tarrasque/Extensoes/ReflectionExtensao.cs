using System;
using System.Linq;
using System.Reflection;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class ReflectionExtensao
    {
        public static Type ObterMemberType(this MemberInfo memberInfo)
        {
            var methodInfo = memberInfo as MethodInfo;
            if (methodInfo != null)
                return methodInfo.ReturnType;
            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
                return propertyInfo.PropertyType;
            var fieldInfo = memberInfo as FieldInfo;
            return fieldInfo?.FieldType;
        }
        public static TAtributo ObterAnotacao<TAtributo>(this MemberInfo propriedade) where TAtributo : Attribute
        {
            return propriedade.GetCustomAttributes<TAtributo>(true).FirstOrDefault();
        }
        public static bool TemAnotacao<TAtributo>(this MemberInfo tipo) where TAtributo : Attribute
        {
            return tipo.GetCustomAttributes<TAtributo>(true).Any();
        }
        public static bool TemAnotacao<TAtributo>(this PropertyInfo propriedade) where TAtributo : Attribute
        {
            return propriedade.GetCustomAttributes<TAtributo>(true).Any();
        }
    }
}
