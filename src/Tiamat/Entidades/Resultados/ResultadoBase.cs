using Nebularium.Tiamat.Abstracoes;
using Nebularium.Tiamat.Enumeracoes;
using Nebularium.Tiamat.Recursos;
using Nebularium.Tiamat.Validacoes;
using System.Collections.Generic;
using System.Linq;

namespace Nebularium.Tiamat.Entidades.Resultados
{
    public abstract class ResultadoBase<T> : IResultado<T>
    {
        public IMetadadoResultado Metadado { get; set; }
        public T Dado { get; set; }
    }

    public class Resultado<T> : ResultadoBase<T>
    {
        public Resultado() { }
        public Resultado(T obj, TipoResultado tipoResultado)
        {
            Metadado = new MetadadoResultado(tipoResultado);
            Dado = obj;
        }
        public static Resultado<T> Retorno(T obj, TipoResultado tipoResultado)
        {
            return new Resultado<T>(obj, tipoResultado);
        }
    }

    public class CriacaoResultado : ResultadoBase<CriacaoDado>
    {
        public CriacaoResultado() { }
        public CriacaoResultado(string id)
        {
            Metadado = new MetadadoResultado(TipoResultado.String);
            Dado = new CriacaoDado(id);
        }
        public static CriacaoResultado Retorno(string id)
        {
            return new CriacaoResultado(id);
        }
    }

    public class SucessoResultado : ResultadoBase<SucessoDado>
    {
        public SucessoResultado() { }
        public SucessoResultado(bool valor)
        {
            Metadado = new MetadadoResultado(TipoResultado.Bool);
            Dado = new SucessoDado(valor);
        }
        public static SucessoResultado Retorno(bool valor)
        {
            return new SucessoResultado(valor);
        }
    }

    public class ListaResultado<T> : ResultadoBase<ListaDado<T>>
    {
        public ListaResultado() { }
        public ListaResultado(List<T> lista, IPaginador paginador)
        {
            Metadado = new MetadadoPaginado(paginador);
            Dado = new ListaDado<T>(lista);
        }
        public static ListaResultado<T> Retorno(List<T> lista, IPaginador paginador)
        {
            return new ListaResultado<T>(lista, paginador);
        }
    }

    public class ObjetoResultado<T> : ResultadoBase<ObjetoDado<T>>
    {
        public ObjetoResultado() { }
        public ObjetoResultado(T obj)
        {
            Metadado = new MetadadoResultado(TipoResultado.Object);
            Dado = new ObjetoDado<T>(obj);
        }
        public static ObjetoResultado<T> Retorno(T obj)
        {
            return new ObjetoResultado<T>(obj);
        }
    }

    public class ObjetoPaginadoResultado<T> : ResultadoBase<ObjetoDado<T>>
    {
        public ObjetoPaginadoResultado() { }
        public ObjetoPaginadoResultado(T obj, IPaginador paginador)
        {
            Metadado = new MetadadoPaginado(paginador);
            Dado = new ObjetoDado<T>(obj);
        }
        public static ObjetoPaginadoResultado<T> Retorno(T obj, IPaginador paginador)
        {
            return new ObjetoPaginadoResultado<T>(obj, paginador);
        }
    }

    public class ErroValidacaoResultado : ResultadoBase<ListaDado<ErroValidacao>>
    {
        public ErroValidacaoResultado() { }
        public ErroValidacaoResultado(List<ErroValidacao> lista, IPaginador paginador)
        {
            Metadado = new MetadadoPaginado(paginador);
            Dado = new ListaDado<ErroValidacao>(lista);
        }
        public static ErroValidacaoResultado Retorno(Dictionary<string, string> erros)
        {
            var paginador = new Paginador { Pagina = 1, TamanhoPagina = erros.Count };
            var lista = erros.Select(n => new ErroValidacao(n.Key, n.Value));

            return new ErroValidacaoResultado(lista.ToList(), paginador);
        }
    }
}
