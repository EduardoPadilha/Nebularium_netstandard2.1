using AutoMapper.Extensions.ExpressionMapping;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tiamat.Validacoes;

namespace Nebularium.Tellurian.Recursos
{
    public class GestorAutoMapper : GestorMapeamento<GestorAutoMapper>
    {
        public GestorAutoMapper()
        {
            AdicionarConfig(cfg => cfg.AddExpressionMapping());
            AdicionarPerfil<ValidadorPerfilMapeamento>();
            AdicionarPerfil<TellurianPerfilMapeamento>();
        }
    }
}
