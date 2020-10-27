﻿using System;

namespace Nebularium.Behemoth.Mongo
{
    [AttributeUsage(AttributeTargets.Class)
]
    public class NomeColecaoAttribute : Attribute
    {
        public string Nome { get; private set; }
        public NomeColecaoAttribute(string nome)
        {
            Nome = nome;
        }
    }
}