using System;

namespace Nebularium.Behemoth.Atributos
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NomeAttribute : Attribute
    {
        public string Nome { get; private set; }
        public NomeAttribute(string nome)
        {
            Nome = nome;
        }
    }
}
