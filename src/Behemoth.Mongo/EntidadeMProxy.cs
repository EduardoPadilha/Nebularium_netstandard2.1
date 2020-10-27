using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebularium.Tiamat.Interfaces;

namespace Nebularium.Behemoth.Mongo
{
    public class EntidadeMProxy : IEntidade
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Equals((IEntidade)obj);
        }

        public virtual bool Equals(EntidadeMProxy obj)
        {
            return obj.Id == Id;
        }

        public override int GetHashCode()
        {
            return $"{ GetType().FullName}-{Id}".GetHashCode();
        }
    }
}
