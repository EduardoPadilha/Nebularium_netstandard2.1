using Nebularium.Tiamat.Interfaces;
using System.ComponentModel;

namespace Nebularium.Tiamat.Entidades
{
    public abstract class Entidade : IEntidade
    {
        [DisplayName]
        public string Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Equals((IEntidade)obj);
        }

        public virtual bool Equals(Entidade obj)
        {
            return obj.Id == Id;
        }

        public override int GetHashCode()
        {
            return $"{ GetType().FullName}-{Id}".GetHashCode();
        }

        //public static bool operator ==(Entidade lhs, Entidade rhs)
        //{
        //    if (Object.ReferenceEquals(lhs, null))
        //    {
        //        if (Object.ReferenceEquals(rhs, null))
        //            return true;

        //        return false;
        //    }
        //    return lhs.Equals(rhs);
        //}

        //public static bool operator !=(Entidade lhs, Entidade rhs)
        //{
        //    return !(lhs == rhs);
        //}
    }
}
