using System;
using System.Text.RegularExpressions;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class StringExtensao
    {
        /// <summary>
        /// Separar o padrao camelCase, ex: TesteCamelCase => Teste Camel Case
        /// </summary>
        public static string separarCamelCase(this string str)
        {
            return string.IsNullOrEmpty(str) ? str : Regex.Replace(str, "([A-Z])", " $1").Trim();
        }
        public static string removerEspacosDuplicados(this string str)
        {
            return Regex.Replace(str, @"\s+", " ");
        }
        public static string removerCaracteresEspeciais(this string str)
        {
            return string.IsNullOrEmpty(str) ? str : Regex.Replace(str, @"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ\s]+?", string.Empty);
        }
        public static string removeAcentos(this string str)
        {
            string C_acentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇçºª&";
            string S_acentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCcoae";
            for (int i = 0; i < C_acentos.Length; i++)
                str = str.Replace(C_acentos[i].ToString(), S_acentos[i].ToString()).Trim();
            return str;
        }
        public static bool limpoNuloBranco(this string s)
        {
            return string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s);
        }
        public static bool limpoNuloBrancoOuZero(this string s)
        {
            return s.limpoNuloBranco() || s == "0";
        }
        public static string obterSeVazioOuBranco(this string s, string padrao = null)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
                return padrao;
            return s;
        }
        public static string obterSeVazioBrancoOuZero(this string s, string padrao = null)
        {
            return s.limpoNuloBrancoOuZero() ? padrao : s;
        }
        public static bool verdadeiro(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            return str.ToUpper() == "S";
        }
        public static string reduzNofim(this string s, int tamanhoReducao)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s) || tamanhoReducao > s.Length || tamanhoReducao < 0)
                return s;
            return s.Substring(0, s.Length - tamanhoReducao);
        }
        public static byte[] paraBytes(this string s)
        {
            if (s == null)
                return new byte[0];
            //byte[] bytes = new byte[s.Length * sizeof(char)];
            //Buffer.BlockCopy(s.ToCharArray(), 0, bytes, 0, bytes.Length);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
            return bytes;
        }
        public static T ToEnum<T>(this string value)
        {
            return Enum.Parse(typeof(T), value, true).como<T>();
        }
    }
}
