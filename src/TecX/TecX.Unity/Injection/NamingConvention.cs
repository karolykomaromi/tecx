namespace TecX.Unity.Injection
{
    using System;
    using System.Collections.Generic;

    using TecX.Common;

    public abstract class NamingConvention
    {
        public NamingConvention Next { get; set; }

        public static NamingConvention CreateForType(Type type)
        {
            Guard.AssertNotNull(type, "type");

            if (type == typeof(string))
            {
                return new ConnectionStringNamingConvention();
            }

            // get all base classes, get all interfaces
            List<Type> allTypes = new List<Type>();

            Type current = type;
            while (current != null && current != typeof(object))
            {
                allTypes.Add(current);
                current = current.BaseType;
            }

            allTypes.AddRange(type.AllInterfaces());

            NamingConvention anchor = null;
            NamingConvention convention = null;

            foreach (Type t in allTypes)
            {
                if (convention == null)
                {
                    convention = new ByTypeNamingConvention(t);
                    anchor = convention;
                }
                else
                {
                    NamingConvention c = new ByTypeNamingConvention(t);
                    convention.Next = c;
                    convention = c;
                }
            }

            return anchor;
        }

        public bool NameMatches(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            if (this.NameMatchesCore(name))
            {
                return true;
            }

            if (this.Next != null)
            {
                return this.Next.NameMatches(name);
            }

            return false;
        }

        protected abstract bool NameMatchesCore(string name);
    }
}