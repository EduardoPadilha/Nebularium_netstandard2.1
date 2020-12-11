using AutoMapper.Extensions.ExpressionMapping;
using Nebularium.Tarrasque.Gestores;
using Nebularium.Tellurian.Recursos;
using Nebularium.Tiamat.Validacoes;

namespace Nebularium.Tellurian.Drone.Recursos
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
