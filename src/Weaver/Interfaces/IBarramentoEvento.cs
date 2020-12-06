namespace Nebularium.Weaver.Interfaces
{
    public interface IBarramentoEvento
    {
        void Publicar(IEvento evento);

        void Assinar<TEvento, TEventoManipulador>()
            where TEvento : IEvento
            where TEventoManipulador : IEventoManipulador<TEvento>;

        void CancelarAssinatura<TEvento, TEventoManipulador>()
            where TEvento : IEvento
            where TEventoManipulador : IEventoManipulador<TEvento>;
    }
}
