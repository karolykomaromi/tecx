using System;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public class NamedInstanceRegistrationExpression : InstanceRegistrationExpression
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

        public NamedInstanceRegistrationExpression(Type from, object instance) 
            : base(from, instance)
        {
            Name = Guid.NewGuid().ToString();
        }

        public NamedInstanceRegistrationExpression Named(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            Name = name;

            return this;
        }

        public override Registration Compile()
        {
            return new InstanceRegistration(From, Name, Instance, Lifetime);
        }
    }
}