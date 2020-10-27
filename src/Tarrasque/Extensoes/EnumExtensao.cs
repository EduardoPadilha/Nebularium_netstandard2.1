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
        public static string Descricao(this Enum tipo)
        {
            if (tipo == null)
                return string.Empty;
            if (tipo.GetType().GetCustomAttribute<FlagsAttribute>() == null)
                return ObterDescricaoEnum(tipo);
            var sb = new StringBuilder();
            var valores = Enum.GetValues(tipo.GetType());
            foreach (var e in valores.Cast<Enum>().Where(tipo.HasFlag))
            {
                sb.Append(ObterDescricaoEnum(e));
                sb.Append(",");
            }
            if (sb.Length > 0)
                sb.Length -= 1;
            return sb.ToString();
        }
        private static string ObterDescricaoEnum(Enum tipo)
        {
            FieldInfo fieldInfo = tipo.GetType().GetField(tipo.ToString());
            if (fieldInfo == null)
                return tipo.ToString().SepararCamelCase();
            var atributo = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            if (atributo != null)
                return atributo.Description;
            var nameAttribute = fieldInfo.GetCustomAttribute<DisplayNameAttribute>();
            if (nameAttribute != null)
                return nameAttribute.DisplayName;
            return tipo.ToString().SepararCamelCase();
        }
        public static string DescricaoUnica(this Enum tipo)
        {
            if (tipo == null)
                return string.Empty;
            return ObterDescricaoEnum(tipo);
        }
        public static string Nome(this Enum tipo)
        {
            return tipo.ToString().ToLower();
        }
        public static string ValorComoString(this Enum tipo)
        {
            int valor = Convert.ToInt32(tipo);
            return valor.ToString();
        }
        public static int Valor(this Enum tipo)
        {
            return Convert.ToInt32(tipo);
        }
        public static IEnumerable ListaChaveValor(this Enum tipo)
        {
            var tipoEnum = tipo.GetType();
            var valores = Enum.GetValues(tipoEnum);
            foreach (var e in valores.Cast<Enum>().Where(tipo.HasFlag))
            {
                yield return new ChaveValor { Valor = e.ValorComoString(), Descricao = e.DescricaoUnica() };
            }
        }
        public static List<T> ObterValores<T>(this T valor)
        {
            var comoEnum = valor.Como<Enum>();
            var r = new List<T>();
            foreach (var v in Enum.GetValues(typeof(T)))
            {
                if (comoEnum.HasFlag(v.Como<Enum>()))
                    r.Add(v.Como<T>());
            }
            return r;
        }
    }
}
