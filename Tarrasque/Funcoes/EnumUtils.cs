using Nebularium.Tarrasque.Extensoes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nebularium.Tarrasque.Funcoes
{
    public static class EnumUtils
    {
        public static IEnumerable listaChaveValor<TEnum>()
        {
            var valores = Enum.GetValues(typeof(TEnum));
            foreach (var e in valores.Cast<Enum>())
            {
                yield return new ChaveValor { Valor = e.valorComoString(), Descricao = e.descricaoUnica() };
            }
        }
        public static IEnumerable listaChaveValor<TEnum>(params TEnum[] filtros)
        {
            var valores = Enum.GetValues(typeof(TEnum));
            var listaEnum = valores.Cast<TEnum>().Where(filtros.Contains);
            foreach (var e in listaEnum.Cast<Enum>())
            {
                yield return new ChaveValor { Valor = e.valorComoString(), Descricao = e.descricaoUnica() };
            }
        }
        public static List<string> listaValores<TEnum>()
        {
            var lista = new List<string>();
            var valores = Enum.GetValues(typeof(TEnum));
            foreach (var e in valores.Cast<Enum>())
            {
                lista.Add(e.descricaoUnica());
            }
            return lista;
        }
        public static List<TEnum> lista<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
        }
        public static IEnumerable listaChaveValorExceto<TEnum>(params TEnum[] filtros)
        {
            var valores = Enum.GetValues(typeof(TEnum));
            var listaEnum = valores.Cast<TEnum>().Where(i => !filtros.Contains(i));
            foreach (var e in listaEnum.Cast<Enum>())
            {
                yield return new ChaveValor { Valor = e.valorComoString(), Descricao = e.descricaoUnica() };
            }
        }
    }
}
