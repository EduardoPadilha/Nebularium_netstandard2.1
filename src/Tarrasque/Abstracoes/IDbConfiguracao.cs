namespace Nebularium.Tarrasque.Abstracoes
{
    public interface IDbConfiguracao : IConfiguracao
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
