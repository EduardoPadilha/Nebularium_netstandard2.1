﻿using Nebularium.Behemoth.Mongo.Contextos;
using Nebularium.Tarrasque.Abstracoes;

namespace Nebularium.Tellurian.Recursos
{
    public class TellurianContext : MongoContextoBase
    {
        public TellurianContext(IDbConfiguracao mongoConfig) : base(mongoConfig)
        {
        }

        public override bool UsarMapeamentoBsonClassMap => false;

        public override void ConfigurarMapeamentoBsonClassMap()
        {
        }
    }
}
