using System;
using System.Collections;

namespace Nebularium.Tarrasque.Interfaces
{
    public interface IExcelsable
    {
        IList Registros { get; set; }
        Type TipoRegsitros { get; }
    }
}
