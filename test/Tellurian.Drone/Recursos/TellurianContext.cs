using Nebularium.Behemoth.Mongo.Contextos;
using Nebularium.Tarrasque.Configuracoes;

namespace Nebularium.Tellurian.Recursos
{
    public class TellurianContext : MongoContextoBase
    {
        public TellurianContext(IDbConfiguracao mongoConfig) : base(mongoConfig)
        {
        }
    }
}
