using System;

namespace Nebularium.Weaver.Interfaces
{
    public interface IEvento
    {
        Guid Id { get; }
        DateTimeOffset CriadoEm { get; }
    }
}
