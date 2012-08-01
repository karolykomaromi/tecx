namespace TecX.Unity.Injection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TecX.Common;

    public abstract class NamingConvention
    {
        public NamingConvention Next { get; set; }

        public static NamingConvention Create(Type type)
        {
            Guard.AssertNotNull(type, "type");

            if (type == typeof(string))
            {
                return new CompositeNamingConvention(new NamingConvention[]
                    {
                        new ConnectionStringNamingConvention(), 
                        new FileNameConvention()
                    });
            }

            IEnumerable<Type> allTypes = type.GetAllBaseClassesAndInterfaces();

            List<NamingConvention> conventions = new List<NamingConvention>(allTypes.Select(t => new ByTypeNamingConvention(t)));

            return new CompositeNamingConvention(conventions);
        }

        public abstract bool NameMatches(string name);
    }
}