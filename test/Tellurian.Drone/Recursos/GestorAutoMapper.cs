using AutoMapper.Extensions.ExpressionMapping;
using Nebularium.Tellurian.Recursos;

namespace Nebularium.Tellurian.Drone.Recursos
{
    public class GestorAutoMapper : Nebularium.Tiamat.Recursos.GestorAutoMapper<GestorAutoMapper>
    {
        public GestorAutoMapper()
        {
            AdicionarConfig(cfg => cfg.AddExpressionMapping());
            AdicionarPerfil<TellurianPerfilMapeamento>();
        }
    }
}
