using System;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public class NamedTypeRegistrationExpression : TypeRegistrationExpression
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                Guard.AssertNotEmpty(value, "Name");
                _name = value;
            }
        }

        public NamedTypeRegistrationExpression(Type from, Type to)
            : base(from, to)
        {
            Name = Guid.NewGuid().ToString();
        }

        public NamedTypeRegistrationExpression Named(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            Name = name;

            return this;
        }

        public override Registration Compile()
        {
            return new TypeRegistration(From, To, Name, Lifetime, Enrichments);
        }
    }
}