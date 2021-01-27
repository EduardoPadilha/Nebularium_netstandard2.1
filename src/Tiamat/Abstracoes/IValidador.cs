﻿using FluentValidation.Internal;
using Nebularium.Tiamat.Validacoes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Nebularium.Tiamat.Abstracoes
{
    public interface IValidador<TEntidade> : IValidador where TEntidade : class
    {
        ValidacaoResultado Validar(TEntidade entidade);
        ValidacaoResultado Validar(TEntidade entidade, string[] rulerSet);
        ValidacaoResultado ValidarPropriedade(TEntidade entidade, Expression<Func<TEntidade, object>> propriedade);
        Action<List<ErroValidacao>> EventoFalhaValidacao { get; set; }
    }
    public interface IValidador
    {
        List<PropertyRule> PegarRegras();
    }
}