using Nebularium.Weaver.Interfaces;
using System;

namespace Nebularium.Weaver
{
    public abstract class Evento : IEvento
    {
        public Guid Id { get; }
        public DateTimeOffset CriadoEm { get; }

        public Evento()
        {
            Id = Guid.NewGuid();
            CriadoEm = DateTimeOffset.UtcNow;
        }
    }
}
