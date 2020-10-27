using System;
using System.Collections.Generic;
using System.Linq;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class ListaExtensao
    {
        public static List<T> RemoverSelecao<T>(this List<T> selecaoRemocao, List<T> listaOriginal, List<T> outraLista)
        {
            if (selecaoRemocao.Count == 0)
                return listaOriginal;
            listaOriginal = listaOriginal.Except(selecaoRemocao).ToList();
            outraLista.AddRange(selecaoRemocao);
            selecaoRemocao.Clear();
            return listaOriginal;
        }
        public static List<T> AdicionarSelecao<T>(this List<T> selecaoAdicao, List<T> listaOriginal, List<T> outraLista)
        {
            if (selecaoAdicao.Count == 0)
                return outraLista;
            listaOriginal.AddRange(selecaoAdicao);
            outraLista = outraLista.Except(listaOriginal).ToList();
            selecaoAdicao.Clear();
            return outraLista;
        }
        public static List<T> MoveSelecao<T>(this List<T> origem, List<T> selecao, List<T> destino)
        {
            if (selecao.Count == 0)
                return origem;
            origem = origem.Except(selecao).ToList();
            destino.AddRange(selecao);
            selecao.Clear();
            return origem;
        }
        public static IEnumerable<T> DistintoPor<T>(this IEnumerable<T> lista, Func<T, object> propriedade)
        {
            return lista.GroupBy(propriedade).Select(x => x.First());
        }
        public static String ObterListaPorSeparador<T>(this IEnumerable<T> lista, String separador, String delimitador = "")
        {
            String separadorFinal = String.Format("{0}{1}{0}", delimitador, separador);
            var listaRetorno = String.Join(separadorFinal, lista);
            String r = String.Format("{0}{1}{0}", delimitador, listaRetorno);
            return r;
        }
    }
}
