namespace TecX.Unity.Configuration.Expressions
{
    using System;

    using TecX.Common;

    public class NamedInstanceRegistrationExpression : InstanceRegistrationExpression
    {
        private string name;

        public NamedInstanceRegistrationExpression(Type from, object instance)
            : base(from, instance)
        {
            this.Name = Guid.NewGuid().ToString();
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                Guard.AssertNotEmpty(value, "Name");

                this.name = value;
            }
        }

        public NamedInstanceRegistrationExpression Named(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            this.Name = name;

            return this;
        }

        public override Registration Compile()
        {
            return new InstanceRegistration(this.From, this.Name, this.Instance, this.Lifetime);
        }
    }
}