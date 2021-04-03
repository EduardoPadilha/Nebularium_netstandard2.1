using System;

namespace Nebularium.Behemoth.Mongo.Mapeamento
{
    public class MetadadoMapeamento
    {
        public bool Ativo { get; set; }
        public DateTimeOffset DataCriacao { get; set; }
        public DateTimeOffset? DataAtualizacao { get; set; }
        public DateTimeOffset? DataDelecao { get; set; }
    }
}
