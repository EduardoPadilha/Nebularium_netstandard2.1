using System.Collections.Generic;

namespace Nebularium.Tiamat.Entidades.Resultados
{
    public class ObjetoDado<T>
    {
        public ObjetoDado() { }
        public ObjetoDado(T resultado)
        {
            Resultado = resultado;
        }

        public T Resultado { get; set; }
    }

    public class CriacaoDado
    {
        public CriacaoDado() { }
        public CriacaoDado(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }

    public class SucessoDado
    {
        public SucessoDado() { }
        public SucessoDado(bool sucesso)
        {
            Sucesso = sucesso;
        }

        public bool Sucesso { get; set; }
    }

    public class ListaDado<T>
    {
        public ListaDado() { }
        public ListaDado(List<T> itens)
        {
            Itens = itens;
        }

        public List<T> Itens { get; set; }
    }
}
