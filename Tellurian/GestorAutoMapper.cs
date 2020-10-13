using Nebularium.Tarrasque.Gestores;
using Nebularium.Tiamat.Validacoes;

namespace Nebularium.Tellurian
{
    public class GestorAutoMapper : GestorMapeamento<GestorAutoMapper>
    {
        public GestorAutoMapper()
        {
            AdicionarPerfil<ValidadorPerfilMapeamento>();
        }
    }
}
