namespace Infrastructure
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Text;

    public static class TypeHelper
    {
        public static string GetSilverlightCompatibleTypeName(Type type)
        {
            Contract.Requires(type != null);

            StringBuilder sb = new StringBuilder(50);

            sb.Append(type.FullName).Append(", ").Append(type.Assembly.FullName);

            return sb.ToString();
        }
    }
}