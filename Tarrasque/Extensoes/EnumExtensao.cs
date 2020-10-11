using Nebularium.Tarrasque.Funcoes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class EnumExtensao
    {
        public static string descricao(this Enum tipo)
        {
            if (tipo == null)
                return string.Empty;
            if (tipo.GetType().GetCustomAttribute<FlagsAttribute>() == null)
                return obterDescricaoEnum(tipo);
            var sb = new StringBuilder();
            var valores = Enum.GetValues(tipo.GetType());
            foreach (var e in valores.Cast<Enum>().Where(tipo.HasFlag))
            {
                sb.Append(obterDescricaoEnum(e));
                sb.Append(",");
            }
            if (sb.Length > 0)
                sb.Length -= 1;
            return sb.ToString();
        }
        private static string obterDescricaoEnum(Enum tipo)
        {
            FieldInfo fieldInfo = tipo.GetType().GetField(tipo.ToString());
            if (fieldInfo == null)
                return tipo.ToString().separarCamelCase();
            var atributo = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            if (atributo != null)
                return atributo.Description;
            var nameAttribute = fieldInfo.GetCustomAttribute<DisplayNameAttribute>();
            if (nameAttribute != null)
                return nameAttribute.DisplayName;
            return tipo.ToString().separarCamelCase();
        }
        public static string descricaoUnica(this Enum tipo)
        {
            if (tipo == null)
                return string.Empty;
            return obterDescricaoEnum(tipo);
        }
        public static string nome(this Enum tipo)
        {
            return tipo.ToString().ToLower();
        }
        public static string valorComoString(this Enum tipo)
        {
            int valor = Convert.ToInt32(tipo);
            return valor.ToString();
        }
        public static int valor(this Enum tipo)
        {
            return Convert.ToInt32(tipo);
        }
        public static IEnumerable listaChaveValor(this Enum tipo)
        {
            var tipoEnum = tipo.GetType();
            var valores = Enum.GetValues(tipoEnum);
            foreach (var e in valores.Cast<Enum>().Where(tipo.HasFlag))
            {
                yield return new ChaveValor { Valor = e.valorComoString(), Descricao = e.descricaoUnica() };
            }
        }
        public static List<T> obterValores<T>(this T valor)
        {
            var comoEnum = valor.como<Enum>();
            var r = new List<T>();
            foreach (var v in Enum.GetValues(typeof(T)))
            {
                if (comoEnum.HasFlag(v.como<Enum>()))
                    r.Add(v.como<T>());
            }
            return r;
        }
    }
}
