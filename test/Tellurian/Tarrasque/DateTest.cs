using Nebularium.Tellurian.Recursos;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Nebularium.Tellurian.Tarrasque
{
    public class DateTest : TesteBase
    {
        public DateTest(ITestOutputHelper saida) : base(saida)
        {
        }

        [Fact]
        public void TimezoneTest()
        {
            var time = TimeZone.CurrentTimeZone;
            Console.WriteLine($"{nameof(time).Comp()}: {time.StandardName}");

            var timezoneLocal = TimeZoneInfo.Local;
            Console.WriteLine($"{nameof(timezoneLocal).Comp()}: {timezoneLocal.StandardName} | {timezoneLocal.BaseUtcOffset} | {timezoneLocal.DisplayName}");

            var dataStringUtc = "2020-12-14 02:56:01.005Z";
            Console.WriteLine($"{nameof(dataStringUtc).Comp()}: {dataStringUtc}");

            DateTimeOffset.TryParse(dataStringUtc, out DateTimeOffset datetimeOffset);
            Console.WriteLine($"{nameof(datetimeOffset).Comp()}: {datetimeOffset}");

            DateTime.TryParse(dataStringUtc, out DateTime datetimeConvert);
            Console.WriteLine($"{nameof(datetimeConvert).Comp()}: {datetimeConvert}");

            var datetime = datetimeOffset.DateTime;
            Console.WriteLine($"{nameof(datetime).Comp()}: {datetime}");

            var datetimeUTC = datetimeOffset.UtcDateTime;
            Console.WriteLine($"{nameof(datetimeUTC).Comp()}: {datetimeUTC}");

            var datetimeLocal = datetimeOffset.LocalDateTime;
            Console.WriteLine($"{nameof(datetimeLocal).Comp()}: {datetimeLocal}");
        }

    }
    public static class StringExtension
    {
        public static string Comp(this string nome)
        {
            return nome.ToLower().PadRight(30);
        }

    }
}
