namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.Data;
    using NHibernate;
    using NHibernate.SqlTypes;
    using NHibernate.UserTypes;

    public class MailChargeUserType : IUserType
    {
        SqlType[] IUserType.SqlTypes
        {
            get { return new[] { NHibernateUtil.String.SqlType }; }
        }

        Type IUserType.ReturnedType
        {
            get { return typeof(MailCharge); }
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

            MailCharge charge;
            if (s == null || !MailCharge.TryParse(s, out charge))
            {
                return MailCharge.Empty;
            }

            return charge;
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            MailCharge charge = value as MailCharge;

            if (charge == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index);

                return;
            }

            NHibernateUtil.String.NullSafeSet(cmd, charge.ToString(), index);
        }

        public object DeepCopy(object value)
        {
            return ((MailCharge)value).Clone();
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