namespace Hydra.Infrastructure.Configuration
{
    using System;
    using System.Data;
    using Hydra.Infrastructure.I18n;
    using NHibernate;
    using NHibernate.SqlTypes;
    using NHibernate.UserTypes;

    public class SettingNameUserType : IUserType
    {
        SqlType[] IUserType.SqlTypes
        {
            get { return new[] { NHibernateUtil.String.SqlType }; }
        }

        Type IUserType.ReturnedType
        {
            get { return typeof(SettingName); }
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

        object IUserType.NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            string s = (string)NHibernateUtil.String.NullSafeGet(rs, names);

            if (s == null)
            {
                return MultiLanguageString.Empty;
            }

            SettingName name;
            if (SettingName.TryParse(s, out name))
            {
                return name;
            }

            return SettingName.Empty;
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
            SettingName original = value as SettingName;

            if (original == null)
            {
                return null;
            }

            SettingName clone = original.Clone();

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