namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.Data;
    using MimeKit;
    using NHibernate;
    using NHibernate.SqlTypes;
    using NHibernate.UserTypes;

    public class MimeMessageUserType : IUserType
    {
        SqlType[] IUserType.SqlTypes
        {
            get { return new[] { NHibernateUtil.String.SqlType }; }
        }

        Type IUserType.ReturnedType
        {
            get { return typeof(MimeMessage); }
        }

        bool IUserType.IsMutable
        {
            get { return true; }
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

            if (s == null)
            {
                return new MimeMessage();
            }

            MimeMessage msg = MimeMessageBuilder.FromString(s);

            return msg;
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            MimeMessage message = value as MimeMessage;

            if (message == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index);

                return;
            }
            
            NHibernateUtil.String.NullSafeSet(cmd, MimeMessageBuilder.ToString(message), index);
        }

        public object DeepCopy(object value)
        {
            return MimeMessageBuilder.Clone((MimeMessage)value);
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