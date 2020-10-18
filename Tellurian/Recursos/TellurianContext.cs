using Nebularium.Behemoth.Mongo;
using Nebularium.Tarrasque.Configuracoes;

namespace Nebularium.Tellurian.Recursos
{
    public class TellurianContext : MongoContext
    {
        public TellurianContext(IDbConfigs mongoConfig) : base(mongoConfig)
        {
        }
    }
}
