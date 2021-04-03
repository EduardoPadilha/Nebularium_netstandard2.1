namespace Nebularium.Tiamat.Abstracoes
{
    public interface IResultado<T>
    {
        IMetadadoResultado Metadado { get; }
        T Dado { get; }
    }

    public interface IMetadadoResultado
    {
        string Tipo { get; }
    }
}
