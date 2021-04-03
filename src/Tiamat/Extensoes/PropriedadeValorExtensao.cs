using Nebularium.Tiamat.Recursos;
using System;
using System.Collections.Generic;

namespace Nebularium.Tiamat.Extensoes
{
    public static class PropriedadeValorExtensao
    {
        public static TEntidade ParaEntidade<TEntidade>(this List<PropriedadeValor> lista)
        {
            var entidade = Activator.CreateInstance<TEntidade>();
            foreach (var prop in lista)
                prop.Info.SetValue(entidade, prop.Valor, null);
            return entidade;
        }
    }
}
