namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.Data;
    using System.Globalization;
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

            MimeMessage msg = (MimeMessage)ConvertHelper.Convert(s, typeof(MimeMessage), CultureInfo.InvariantCulture);

            return msg;
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index);

                return;
            }

            value = ConvertHelper.Convert(value, typeof(string), CultureInfo.InvariantCulture);

            NHibernateUtil.String.NullSafeSet(cmd, value, index);
        }

        public object DeepCopy(object value)
        {
            string s = (string)ConvertHelper.Convert(value, typeof(string), CultureInfo.InvariantCulture);

            MimeMessage copy = (MimeMessage)ConvertHelper.Convert(s, typeof(MimeMessage), CultureInfo.InvariantCulture);

            return copy;
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