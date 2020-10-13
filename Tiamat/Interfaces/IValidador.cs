using FluentValidation.Internal;
using Nebularium.Tiamat.Validacoes;
using System.Collections.Generic;

namespace Nebularium.Tiamat.Interfaces
{
    public interface IValidador<in TEntidade> : IValidador where TEntidade : class
    {
        ValidacaoResultado Validar(TEntidade entidade);
    }
    public interface IValidador
    {
        List<PropertyRule> PegarRegras();
    }
}
