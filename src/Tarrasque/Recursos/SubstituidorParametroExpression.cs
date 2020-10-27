using System.Collections.Generic;
using System.Linq.Expressions;

namespace Nebularium.Tarrasque.Recursos
{
    public class SubstituidorParametroExpression : ExpressionVisitor
    {
        private IDictionary<ParameterExpression, ParameterExpression> ParametrosSubsituticao { get; set; }

        public SubstituidorParametroExpression(IList<ParameterExpression> parametrosOrigem, IList<ParameterExpression> parametrosDestino)
        {
            ParametrosSubsituticao = new Dictionary<ParameterExpression, ParameterExpression>();

            for (int i = 0; i != parametrosOrigem.Count && i != parametrosDestino.Count; i++)
                ParametrosSubsituticao.Add(parametrosOrigem[i], parametrosDestino[i]);
        }

        protected override Expression VisitParameter(ParameterExpression parametroNo)
        {
            if (ParametrosSubsituticao.TryGetValue(parametroNo, out ParameterExpression substituicao))
                parametroNo = substituicao;

            return base.VisitParameter(parametroNo);
        }
    }
}
