namespace Hydra.Nh.Infrastructure.I18n
{
    using System;
    using System.Data;
    using Hydra.Infrastructure.I18n;
    using NHibernate;
    using NHibernate.SqlTypes;
    using NHibernate.UserTypes;

    public class MultiLanguageStringUserType : IUserType
    {
        SqlType[] IUserType.SqlTypes
        {
            get { return new[] { NHibernateUtil.String.SqlType }; }
        }

        Type IUserType.ReturnedType
        {
            get { return typeof(MultiLanguageString); }
        }

        bool IUserType.IsMutable
        {
            get { return true; }
        }

        bool IUserType.Equals(object x, object y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null)
            {
                return false;
            }

            if (y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        int IUserType.GetHashCode(object x)
        {
            if (x == null)
            {
                return 0;
            }

            return x.GetHashCode();
        }

        object IUserType.NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            string s = (string)NHibernateUtil.String.NullSafeGet(rs, names);

            if (s == null)
            {
                return MultiLanguageString.Empty;
            }

            MultiLanguageString mls;
            if (MultiLanguageString.TryParse(s, out mls))
            {
                return mls;
            }

            return MultiLanguageString.Empty;
        }

        void IUserType.NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index);

                return;
            }

            value = value.ToString();

            NHibernateUtil.String.NullSafeSet(cmd, value, index);
        }

        object IUserType.DeepCopy(object value)
        {
            MultiLanguageString original = value as MultiLanguageString;

            if (original == null)
            {
                return null;
            }

            MultiLanguageString clone = original.Clone();

            return clone;
        }

        object IUserType.Replace(object original, object target, object owner)
        {
            return original;
        }

        object IUserType.Assemble(object cached, object owner)
        {
            return cached;
        }

        object IUserType.Disassemble(object value)
        {
            return value;
        }
    }
}
