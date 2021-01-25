using System;
using System.Text.RegularExpressions;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class StringExtensao
    {
        /// <summary>
        /// Separar o padrao camelCase, ex: TesteCamelCase => Teste Camel Case
        /// </summary>
        public static string SepararCamelCase(this string str)
        {
            return string.IsNullOrEmpty(str) ? str : Regex.Replace(str, "([A-Z])", " $1").Trim();
        }
        public static string RemoverEspacosDuplicados(this string str)
        {
            return Regex.Replace(str, @"\s+", " ");
        }
        public static string RemoverCaracteresEspeciais(this string str)
        {
            return string.IsNullOrEmpty(str) ? str : Regex.Replace(str, @"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ\s]+?", string.Empty);
        }
        public static string RemoveAcentos(this string str)
        {
            string C_acentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇçºª&";
            string S_acentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCcoae";
            for (int i = 0; i < C_acentos.Length; i++)
                str = str.Replace(C_acentos[i].ToString(), S_acentos[i].ToString()).Trim();
            return str;
        }
        public static bool LimpoNuloBranco(this string s)
        {
            return string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s);
        }
        public static bool limpoNuloBrancoOuZero(this string s)
        {
            return s.LimpoNuloBranco() || s == "0";
        }
        public static string ObterSeVazioOuBranco(this string s, string padrao = null)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
                return padrao;
            return s;
        }
        public static string ObterSeVazioBrancoOuZero(this string s, string padrao = null)
        {
            return s.limpoNuloBrancoOuZero() ? padrao : s;
        }
        public static bool Verdadeiro(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            return str.ToUpper() == "S";
        }
        public static string ReduzNofim(this string s, int tamanhoReducao)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s) || tamanhoReducao > s.Length || tamanhoReducao < 0)
                return s;
            return s.Substring(0, s.Length - tamanhoReducao);
        }
        public static byte[] ParaBytes(this string s)
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
            return Enum.Parse(typeof(T), value, true).Como<T>();
        }
        /// <summary>
        /// Retorna as iniciais do nome.
        /// Sem o segundo parâmetro serão retornadas apenas a primeira e a última inicial.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="todas">Todas para retornar todas iniciais possíveis</param>
        /// <returns></returns>
        public static string ObterIniciais(this String s, bool todas = false)
        {
            s = s.RemoveAcentos();
            s = Regex.Replace(s, @"( da| das| do| dos)", "");
            s = Regex.Replace(s, @"(\B\w\s*)", "");
            if (todas)
                return s.ToUpperInvariant();
            return string.Format("{0}{1}", s.Substring(0, 1), s.Substring(s.Length - 1, 1));
        }

        public static bool ValidarCnpj(this string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
            {
                return false;
            }

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        public static string FormataCNPJ(this string cnpj)
        {
            return cnpj.RemoverCaracteresEspeciais().Como<long>().ToString(@"00\.000\.000\/0000\-00");
        }

        public static string FormataCPF(this string cnpj)
        {
            return cnpj.RemoverCaracteresEspeciais().Como<long>().ToString(@"000\.000\.000\-00");
        }
        public static string TrataNomeArquivo(this string str)
        {
            return string.IsNullOrEmpty(str) ? str : Regex.Replace(str, @"[^0-9a-zA-Z-._\s]+?", string.Empty);
        }
        public static string GerarNomeArquivo(this string prefixo, string extensao, bool datado = true, string formatoData = "ddMMyyyHHmmss")
        {
            string dataFormatada = "";
            if (datado)
                dataFormatada = $"-{DateTimeOffset.Now.ToDatetimeBr().ToString(formatoData)}";

            return $"{prefixo.TrataNomeArquivo()}{dataFormatada}.{extensao.RemoverCaracteresEspeciais()}";
        }
    }
}
