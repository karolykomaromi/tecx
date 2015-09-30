namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.Data;
    using NHibernate;
    using NHibernate.SqlTypes;
    using NHibernate.UserTypes;

    public class MailBagUserType : IUserType
    {
        SqlType[] IUserType.SqlTypes
        {
            get { return new[] { NHibernateUtil.String.SqlType }; }
        }

        Type IUserType.ReturnedType
        {
            get { return typeof(MailBag); }
        }

        bool IUserType.IsMutable
        {
            get { return false; }
        }

        bool IUserType.Equals(object x, object y)
        {
            return EqualityComparer.Equals(x, y);
        }

        int IUserType.GetHashCode(object x)
        {
            if (x == null)
            {
                return 0;
            }

            return EqualityComparer.Default(x.GetType()).GetHashCode(x);
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            string s = (string)NHibernateUtil.String.NullSafeGet(rs, names);

            MailBag bag;
            if (s == null || !MailBag.TryParse(s, out bag))
            {
                return MailBag.Empty;
            }

            return bag;
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            MailBag bag = value as MailBag;

            if (bag == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index);

                return;
            }

            NHibernateUtil.String.NullSafeSet(cmd, bag.ToString(), index);
        }

        public object DeepCopy(object value)
        {
            return ((MailBag)value).Clone();
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }
    }
}