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
        public static double sempreConverteDouble(object vl, double padraoParaNulo = 0)
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
        public static decimal sempreConverteDecimal(object vl, decimal padraoParaNulo = 0)
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
        public static int sempreConverteInt32(object vl, int padraoParaNulo = 0)
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
        public static long sempreConverteInt64(object vl, long padraoParaNulo = 0)
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
        public static DateTime sempreConverteDate(object vl, DateTime padraoParaNulo = default)
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
        public static bool sempreConverteBoleano(object vl, bool padraoParaNulo = false)
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
        public static string sempreConverteString(object vl, string padraoParaNulo = "")
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
        public static T sempreConverte<T>(object vl, T padraoParaNulo = default)
        {
            //            if (typeof(T) == typeof(String))
            //                return sempreConverteString(vl, padraoParaNulo.ToString()).como<T>();
            if (typeof(T) == typeof(double))
                return sempreConverteDouble(vl, padraoParaNulo.como<double>()).como<T>();
            if (typeof(T) == typeof(decimal))
                return sempreConverteDecimal(vl, padraoParaNulo.como<decimal>()).como<T>();
            if (typeof(T) == typeof(int))
                return sempreConverteInt32(vl, padraoParaNulo.como<int>()).como<T>();
            if (typeof(T) == typeof(long))
                return sempreConverteInt64(vl, padraoParaNulo.como<long>()).como<T>();
            if (typeof(T) == typeof(bool))
                return sempreConverteBoleano(vl, padraoParaNulo.como<bool>()).como<T>();
            if (typeof(T) == typeof(DateTime))
                return sempreConverteDate(vl, padraoParaNulo.como<DateTime>()).como<T>();
            return padraoParaNulo;
        }
        public static object sempreConverte(object vl, Type tipo, object padraoParaNulo = default)
        {
            if (vl == null)
                return padraoParaNulo;
            if (tipo == typeof(string))
                return vl.ToString().limpoNuloBranco() ? padraoParaNulo : vl.ToString();
            if (tipo == typeof(int) || tipo == typeof(int?))
                return sempreConverteInt32(vl, padraoParaNulo.como<int>());
            if (tipo == typeof(decimal) || tipo == typeof(decimal?))
                return sempreConverteDecimal(vl, padraoParaNulo.como<decimal>());
            if (tipo == typeof(bool) || tipo == typeof(bool?))
                return sempreConverteBoleano(vl, padraoParaNulo.como<bool>());
            if (tipo == typeof(DateTime) || tipo == typeof(DateTime?))
                return sempreConverteDate(vl, padraoParaNulo.como<DateTime>());
            if (tipo == typeof(double) || tipo == typeof(double?))
                return sempreConverteDouble(vl, padraoParaNulo.como<double>());
            if (tipo == typeof(long) || tipo == typeof(long?))
                return sempreConverteInt64(vl, padraoParaNulo.como<long>());
            return padraoParaNulo;
        }
        public static T sempreConverteEnum<T>(object vl, T padraoParaNulo = default)
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
        public static T sempreConverteEnumPorString<T>(string valor, bool caseSensitive = false)
        {
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                if (caseSensitive)
                {
                    if ((e as Enum).descricao().Equals(valor)) return e.como<T>();
                }
                else
                    if ((e as Enum).descricao().ToLower().Equals(valor.ToLower())) return e.como<T>();
            }
            return 0.como<T>();
        }
    }
}
