namespace TecX.Unity.Configuration.Expressions
{
    using System;

    using TecX.Common;

    public class NamedTypeRegistrationExpression : TypeRegistrationExpression
    {
        private string name;

        public NamedTypeRegistrationExpression(Type from, Type to)
            : base(from, to)
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

        public NamedTypeRegistrationExpression Named(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            this.Name = name;

            return this;
        }

        public override Registration Compile()
        {
            return new TypeRegistration(this.From, this.To, this.Name, this.Lifetime, this.Enrichments);
        }
    }
}