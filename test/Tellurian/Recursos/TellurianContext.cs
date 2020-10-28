using Nebularium.Behemoth.Mongo.Contextos;
using Nebularium.Tarrasque.Configuracoes;

namespace Nebularium.Tellurian.Recursos
{
    public class TellurianContext : MongoContexto
    {
        public TellurianContext(IDbConfigs mongoConfig) : base(mongoConfig)
        {
        }
    }
}
