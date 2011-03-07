using System;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public class NamedInstanceRegistrationExpression<TFrom> : InstanceRegistrationExpression<TFrom>
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

        public NamedInstanceRegistrationExpression(object instance) 
            : base(instance)
        {
            Name = Guid.NewGuid().ToString();
        }

        public NamedInstanceRegistrationExpression<TFrom> Named(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            Name = name;

            return this;
        }

        public override Registration Compile()
        {
            throw new NotImplementedException();
        }
    }
}