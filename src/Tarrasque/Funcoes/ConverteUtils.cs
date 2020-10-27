using Nebularium.Tarrasque.Extensoes;
using System;

namespace Nebularium.Tarrasque.Funcoes
{
    public static class ConverteUtils
    {
        private static Type[] TiposPrimitivos =
        {
            typeof (DateTime),
            typeof (string),
            typeof (bool),
            typeof (decimal),
            typeof (double),
            typeof (int),
            typeof (long),
            typeof (float),
            typeof (int),
            typeof (string),
            typeof (bool),
            typeof (decimal),
            typeof (bool)
        };
        private static Type[] TiposPrimitivosNulllables =
        {
            typeof (DateTime?),
            typeof (bool?),
            typeof (decimal?),
            typeof (double?),
            typeof (int?),
            typeof (long?),
            typeof (float?),
            typeof (int?),
            typeof (bool?),
            typeof (decimal?),
            typeof (bool?)
        };
        public static double SempreConverteDouble(object vl, double padraoParaNulo = 0)
        {
            double r = 0;
            try
            {
                r = Convert.ToDouble(vl);
            }
            catch
            {
                r = padraoParaNulo;
            }
            return r;
        }
        public static decimal SempreConverteDecimal(object vl, decimal padraoParaNulo = 0)
        {
            decimal r = 0;
            try
            {
                r = Convert.ToDecimal(vl);
            }
            catch
            {
                r = padraoParaNulo;
            }
            return r;
        }
        public static int SempreConverteInt32(object vl, int padraoParaNulo = 0)
        {
            int r = 0;
            try
            {
                r = Convert.ToInt32(vl);
            }
            catch
            {
                r = padraoParaNulo;
            }
            return r;
        }
        public static long SempreConverteInt64(object vl, long padraoParaNulo = 0)
        {
            long r = 0;
            try
            {
                r = Convert.ToInt64(vl);
            }
            catch
            {
                r = padraoParaNulo;
            }
            return r;
        }
        public static DateTime SempreConverteDate(object vl, DateTime padraoParaNulo = default)
        {
            DateTime r;
            try
            {
                r = Convert.ToDateTime(vl);
            }
            catch
            {
                r = padraoParaNulo;
            }
            return r;
        }
        public static bool SempreConverteBoleano(object vl, bool padraoParaNulo = false)
        {
            bool r;
            try
            {
                r = Convert.ToBoolean(vl);
            }
            catch
            {
                r = padraoParaNulo;
            }
            return r;
        }
        public static string SempreConverteString(object vl, string padraoParaNulo = "")
        {
            string r;
            try
            {
                r = Convert.ToString(vl);
            }
            catch
            {
                r = padraoParaNulo;
            }
            return r;
        }
        public static T SempreConverte<T>(object vl, T padraoParaNulo = default)
        {
            //            if (typeof(T) == typeof(String))
            //                return sempreConverteString(vl, padraoParaNulo.ToString()).como<T>();
            if (typeof(T) == typeof(double))
                return SempreConverteDouble(vl, padraoParaNulo.Como<double>()).Como<T>();
            if (typeof(T) == typeof(decimal))
                return SempreConverteDecimal(vl, padraoParaNulo.Como<decimal>()).Como<T>();
            if (typeof(T) == typeof(int))
                return SempreConverteInt32(vl, padraoParaNulo.Como<int>()).Como<T>();
            if (typeof(T) == typeof(long))
                return SempreConverteInt64(vl, padraoParaNulo.Como<long>()).Como<T>();
            if (typeof(T) == typeof(bool))
                return SempreConverteBoleano(vl, padraoParaNulo.Como<bool>()).Como<T>();
            if (typeof(T) == typeof(DateTime))
                return SempreConverteDate(vl, padraoParaNulo.Como<DateTime>()).Como<T>();
            return padraoParaNulo;
        }
        public static object SempreConverte(object vl, Type tipo, object padraoParaNulo = default)
        {
            if (vl == null)
                return padraoParaNulo;
            if (tipo == typeof(string))
                return vl.ToString().LimpoNuloBranco() ? padraoParaNulo : vl.ToString();
            if (tipo == typeof(int) || tipo == typeof(int?))
                return SempreConverteInt32(vl, padraoParaNulo.Como<int>());
            if (tipo == typeof(decimal) || tipo == typeof(decimal?))
                return SempreConverteDecimal(vl, padraoParaNulo.Como<decimal>());
            if (tipo == typeof(bool) || tipo == typeof(bool?))
                return SempreConverteBoleano(vl, padraoParaNulo.Como<bool>());
            if (tipo == typeof(DateTime) || tipo == typeof(DateTime?))
                return SempreConverteDate(vl, padraoParaNulo.Como<DateTime>());
            if (tipo == typeof(double) || tipo == typeof(double?))
                return SempreConverteDouble(vl, padraoParaNulo.Como<double>());
            if (tipo == typeof(long) || tipo == typeof(long?))
                return SempreConverteInt64(vl, padraoParaNulo.Como<long>());
            return padraoParaNulo;
        }
        public static T SempreConverteEnum<T>(object vl, T padraoParaNulo = default)
        {
            try
            {
                var r = (T)Enum.Parse(typeof(T), vl.ToString(), true);
                return r;
            }
            catch (Exception)
            {
                return padraoParaNulo.Equals(default(T)) ? (T)Enum.GetValues(typeof(T)).GetValue(0) : padraoParaNulo;
            }
        }
        public static T SempreConverteEnumPorString<T>(string valor, bool caseSensitive = false)
        {
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                if (caseSensitive)
                {
                    if ((e as Enum).Descricao().Equals(valor)) return e.Como<T>();
                }
                else
                    if ((e as Enum).Descricao().ToLower().Equals(valor.ToLower())) return e.Como<T>();
            }
            return 0.Como<T>();
        }
    }
}
