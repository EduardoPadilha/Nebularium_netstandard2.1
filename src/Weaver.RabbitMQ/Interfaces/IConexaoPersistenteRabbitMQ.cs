using RabbitMQ.Client;

namespace Nebularium.Weaver.RabbitMQ.Interfaces
{
    public interface IConexaoPersistenteRabbitMQ
    {
        bool EstaConectado { get; }
        bool TentaConectar();
        IModel CriaModelo();
    }
}
