using Nebularium.Tarrasque.Interfaces;
using System;

namespace Nebularium.Tarrasque.Configuracoes
{
    public class Configuracao
    {
        //public class ConexoesConfiguracao
        //{
        //    public const string Conexoes = "Position";


        //}
        public static Func<IDisplayNameExtrator> DisplayNameExtrator { get; set; }
    }
}
