namespace Nebularium.Weaver.Interfaces
{
    public interface IBarramentoEvento
    {
        void Publicar(IEvento evento);

        void Assinar<TEvento, TEventoManipulador>()
            where TEvento : IEvento
            where TEventoManipulador : IManipuladorEvento<TEvento>;

        void CancelarAssinatura<TEvento, TEventoManipulador>()
            where TEvento : IEvento
            where TEventoManipulador : IManipuladorEvento<TEvento>;
    }
}
