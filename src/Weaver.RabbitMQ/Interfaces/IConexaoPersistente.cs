using RabbitMQ.Client;

namespace Nebularium.Weaver.RabbitMQ.Interfaces
{
    public interface IConexaoPersistente
    {
        bool EstaConectado { get; }
        bool TentaConectar();
        IModel CriaModelo();
    }
}
