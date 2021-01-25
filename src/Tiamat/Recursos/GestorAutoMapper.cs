using Nebularium.Tarrasque.Abstracoes;
using Nebularium.Tarrasque.Gestores;

namespace Nebularium.Tiamat.Recursos
{
    public class GestorAutoMapper<T> : GestorMapeamento<T> where T : IGestorMapeamento, new()
    {
        public GestorAutoMapper()
        {
            AdicionarPerfil<PerfilMapeamento>();
        }
    }
}
