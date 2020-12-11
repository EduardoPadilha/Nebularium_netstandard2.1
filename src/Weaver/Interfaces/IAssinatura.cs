using System;
using System.Threading.Tasks;

namespace Nebularium.Weaver.Interfaces
{
    public interface IAssinatura
    {
        Type TipoEvento { get; }
        Type TipoManipulador { get; }

        Task Resolver(string mensagem);
    }
}