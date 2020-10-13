using System;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class DataHoraExtensao
    {
        public static DateTime DataHoraBelem(this DateTime dt)
        {
            return TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time"));
        }
        public static String TotalEmHorasMinutos(this TimeSpan ts)
        {
            if (ts.Days == 0)
                return ts.ToString(@"hh\:mm");
            var hr = ts.TotalHours.Como<int>();
            hr = hr < 0 ? hr * -1 : hr;
            var min = ts.Minutes;
            min = min < 0 ? min * -1 : min;
            return String.Format("{0:D2}:{1:D2}", hr, min);
        }
        public static string ObterEmString(this DateTime data, string padrao = "dd/MM/yyyy")
        {
            var s = "{0:" + padrao + "}";
            return string.Format(s, data);
        }
        public static DateTime FimDoDia(this DateTime data)
        {
            return data.Date.AddDays(1).AddSeconds(-1);
        }
    }
}
