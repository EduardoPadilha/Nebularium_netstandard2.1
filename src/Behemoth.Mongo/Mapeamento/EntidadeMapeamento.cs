using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebularium.Tiamat.Abstracoes;

namespace Nebularium.Behemoth.Mongo.Mapeamento
{
    public abstract class EntidadeMapeamento : IEntidade
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public MetadadoMapeamento Metadado { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Equals((EntidadeMapeamento)obj);
        }

        public virtual bool Equals(EntidadeMapeamento obj)
        {
            return obj.Id == Id;
        }

        public override int GetHashCode()
        {
            return $"{ GetType().FullName}-{Id}".GetHashCode();
        }
    }
}
