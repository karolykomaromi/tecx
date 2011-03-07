using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public class NamedTypeRegistrationExpression<TFrom, TTo> : TypeRegistrationExpression<TFrom, TTo>
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                Guard.AssertNotEmpty(value, "Name");
                _name = value;
            }
        }

        public NamedTypeRegistrationExpression()
        {
            Name = Guid.NewGuid().ToString();
        }

        public NamedTypeRegistrationExpression<TFrom, TTo> Named(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            Name = name;

            return this;
        }

        public override Registration Compile()
        {
            return new TypeRegistration(typeof (TFrom), typeof (TTo), Name, Lifetime, Enrichments);
        }
    }
}