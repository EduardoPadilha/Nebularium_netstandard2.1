using Nebularium.Tarrasque.Extensoes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nebularium.Tarrasque.Funcoes
{
    public static class EnumUtils
    {
        public static IEnumerable ListaChaveValor<TEnum>()
        {
            var valores = Enum.GetValues(typeof(TEnum));
            foreach (var e in valores.Cast<Enum>())
            {
                yield return new ChaveValor { Valor = e.ValorComoString(), Descricao = e.DescricaoUnica() };
            }
        }
        public static IEnumerable ListaChaveValor<TEnum>(params TEnum[] filtros)
        {
            var valores = Enum.GetValues(typeof(TEnum));
            var listaEnum = valores.Cast<TEnum>().Where(filtros.Contains);
            foreach (var e in listaEnum.Cast<Enum>())
            {
                yield return new ChaveValor { Valor = e.ValorComoString(), Descricao = e.DescricaoUnica() };
            }
        }
        public static List<string> ListaValores<TEnum>()
        {
            var lista = new List<string>();
            var valores = Enum.GetValues(typeof(TEnum));
            foreach (var e in valores.Cast<Enum>())
            {
                lista.Add(e.DescricaoUnica());
            }
            return lista;
        }
        public static List<TEnum> Lista<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
        }
        public static IEnumerable ListaChaveValorExceto<TEnum>(params TEnum[] filtros)
        {
            var valores = Enum.GetValues(typeof(TEnum));
            var listaEnum = valores.Cast<TEnum>().Where(i => !filtros.Contains(i));
            foreach (var e in listaEnum.Cast<Enum>())
            {
                yield return new ChaveValor { Valor = e.ValorComoString(), Descricao = e.DescricaoUnica() };
            }
        }
    }
}
