using System;
using System.Linq;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class DateTimeOffsetExtensao
    {
        private static TimeZoneInfo TimeZoneBR;
        private static string[] TimeZonesBR = new string[] { "E. South America Standard Time", "America/Sao_Paulo" };

        static DateTimeOffsetExtensao()
        {
            var tzs = TimeZoneInfo.GetSystemTimeZones().Where(c => TimeZonesBR.Contains(c.Id));
            if (tzs == null || !tzs.Any())
                throw new TimeZoneNotFoundException("Não foram encontrados TimeZones BR pré-configurados.");

            TimeZoneBR = tzs.First();
        }

        public static DateTimeOffset ToDatetimeBr(this DateTimeOffset date)
        {
            return TimeZoneInfo.ConvertTime(date, TimeZoneBR);
        }

        public static DateTimeOffset ToDatetimeBr(this DateTimeOffset? date)
        {
            if (!date.HasValue) return default;
            return date.Value.ToDatetimeBr();
        }

        public static DateTimeOffset ComoInicioFiltro(this DateTimeOffset date)
        {
            return new DateTimeOffset(date.Date, date.Offset);
        }

        public static DateTimeOffset ComoInicioFiltro(this DateTimeOffset? date)
        {
            if (!date.HasValue) return default;
            return date.Value.ComoInicioFiltro();
        }

        public static DateTimeOffset ComoFimFiltro(this DateTimeOffset date)
        {
            return new DateTimeOffset(date.Date.AddDays(1), date.Offset);
        }

        public static DateTimeOffset ComoFimFiltro(this DateTimeOffset? date)
        {
            if (!date.HasValue) return default;
            return date.Value.ComoFimFiltro();
        }

        public static DateTimeOffset ComoInicioFiltroBr(this DateTimeOffset date)
        {
            return date.ToDatetimeBr().ComoInicioFiltro();
        }

        public static DateTimeOffset ComoInicioFiltroBr(this DateTimeOffset? date)
        {
            if (!date.HasValue) return default;
            return date.Value.ComoInicioFiltroBr();
        }

        public static DateTimeOffset ComoFimFiltroBr(this DateTimeOffset date)
        {
            return date.ToDatetimeBr().ComoFimFiltro();
        }

        public static DateTimeOffset ComoFimFiltroBr(this DateTimeOffset? date)
        {
            if (!date.HasValue) return default;
            return date.Value.ComoFimFiltroBr();
        }

        public static DateTimeOffset ObterInicioMesBr(this DateTimeOffset date)
        {
            return date.ToDatetimeBr().ObterInicioMes();
        }

        public static DateTimeOffset ObterInicioMesBr(this DateTimeOffset? date)
        {
            if (!date.HasValue) return default;
            return date.Value.ObterInicioMesBr();
        }

        public static DateTimeOffset ObterInicioMes(this DateTimeOffset date)
        {
            return new DateTimeOffset(new DateTime(date.Year, date.Month, 1), date.Offset);
        }

        public static DateTimeOffset ObterInicioMes(this DateTimeOffset? date)
        {
            if (!date.HasValue) return default;
            return date.Value.ObterInicioMes();
        }

        public static (DateTimeOffset Inicio, DateTimeOffset Fim) ObterInicioFimMes(this DateTimeOffset date)
        {
            var inicioMes = date.ObterInicioMes();
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);
            return (inicioMes, fimMes);
        }

        public static (DateTimeOffset Inicio, DateTimeOffset Fim) ObterInicioFimMes(this DateTimeOffset? date)
        {
            if (!date.HasValue) return default;
            return date.Value.ObterInicioFimMes();
        }

        public static (DateTimeOffset Inicio, DateTimeOffset Fim) ObterInicioFimMesBr(this DateTimeOffset date)
        {
            return date.ToDatetimeBr().ObterInicioFimMes();
        }

        public static (DateTimeOffset Inicio, DateTimeOffset Fim) ObterInicioFimMesBr(this DateTimeOffset? date)
        {
            if (!date.HasValue) return default;
            return date.Value.ObterInicioFimMesBr();
        }

        public static bool MinimoValor(this DateTimeOffset date)
        {
            return date == default;
        }

        //TODO: Criar extension para filtro data inicio e fim
        //public static Expression<Func<T, bool>> FiltrarRange<T>(this Expression<Func<T, bool>> obj, Expression<Func<T, DateTimeOffset>> propriedade, DateTimeOffset inicio, DateTimeOffset fim)
        //{
        //    //if (propriedade.Body.NodeType != ExpressionType.MemberAccess)
        //    //    throw new ArgumentException($"Propriedade {propriedade.Name} não é um membro acessor (get)");
        //    //var data = (DateTimeOffset)typeof(T).GetProperty(((MemberExpression)propriedade.Body).Member.Name).GetValue(obj, null);
        //    var inicioSemHora = inicio.ComoInicioFiltroBr();
        //    var fimSemHora = fim.ComoFimFiltroBr();
        //    Expression<Func<T, DateTimeOffset, bool>> filtro = (origem, data) => data >= inicioSemHora && data < fimSemHora;
        //    //Expression<Func<T, bool>> lambda = origem => filtro(origem, propriedade(origem));
        //    //var predicado = lambda.Compile();
        //    return obj.And(c => lamb);
        //}
    }
}
