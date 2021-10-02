using Nebularium.Tiamat.Abstracoes;
using System;
using System.ComponentModel;

namespace Nebularium.Tiamat.Entidades
{
    public abstract class Entidade : IEntidade, IEquatable<Entidade>
    {
        public Entidade()
        {
            Metadado = new Metadado();
        }

        [DisplayName]
        public string Id { get; set; }
        public Metadado Metadado { get; set; }

        #region IEquatable<Entidade>

        public override bool Equals(object outra)
        {
            if (outra == null || !GetType().Equals(outra.GetType()))
                return false;

            return Equals((Entidade)outra);
        }

        public virtual bool Equals(Entidade outra)
        {
            return Id.Equals(outra.Id);
        }

        public override int GetHashCode()
        {
            return $"{ GetType().FullName}-{Id}".GetHashCode();
        }

        #endregion

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
