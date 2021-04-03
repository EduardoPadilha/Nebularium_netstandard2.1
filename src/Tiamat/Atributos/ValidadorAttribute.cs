using Nebularium.Tiamat.Abstracoes;
using System;

namespace Nebularium.Tiamat.Atributos
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidadorAttribute : Attribute
    {
        private Type tipo;
        public string[] Cenario { get; set; }
        public bool SoltarExcecao { get; set; }

        public Type Tipo => tipo;

        public ValidadorAttribute(Type tipo) : this()
        {
            this.tipo = tipo;
        }

        public ValidadorAttribute()
        {
            Cenario = null;
            SoltarExcecao = true;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class NaoValidarAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class ValidacaoSimplesAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class ValidarParametroAttribute : Attribute
    {
        public IValidador Validador { get; }
        public string[] Cenario { get; set; }
        public bool SoltarExcecao { get; set; }
        public ValidarParametroAttribute(IValidador validador)
        {
            Validador = validador;
        }
    }
}
