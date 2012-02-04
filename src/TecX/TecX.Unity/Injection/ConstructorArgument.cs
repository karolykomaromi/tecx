namespace TecX.Unity.Injection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TecX.Common;

    public class ConstructorArgument
    {
        private readonly Func<string, bool> nameMatches;

        public ConstructorArgument(string argumentName, object value)
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            this.nameMatches = name => string.Equals(argumentName, name, StringComparison.InvariantCultureIgnoreCase);
            this.Value = value;
        }

        public ConstructorArgument(object value)
        {
            Guard.AssertNotNull(value, "value");
            this.Value = value;
            this.nameMatches = new DefaultNamingConvention(value.GetType()).NameMatches;
        }

        public bool NameMatches(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            return this.nameMatches(name);
        }

        public object Value { get; set; }
    }

    public class DefaultNamingConvention
    {
        private readonly Type type;

        private readonly List<string> snippets;

        public DefaultNamingConvention(Type type)
        {
            Guard.AssertNotNull(type, "type");
            this.type = type;
            this.snippets = new List<string>();
            this.InitializeSnippets();
        }

        public bool NameMatches(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            return this.snippets.Any(s => string.Equals(s, name, StringComparison.InvariantCultureIgnoreCase));
        }

        private void InitializeSnippets()
        {
            string typeName = this.type.Name;

            if (typeName.StartsWith("i", StringComparison.InvariantCultureIgnoreCase))
            {
                typeName = typeName.Substring(1);
            }

            for (int i = 0; i < typeName.Length; i++)
            {
                char c = typeName[i];

                if (char.IsUpper(c))
                {
                    string snippet = typeName.Substring(i);
                    this.snippets.Add(snippet);
                }
            }
        }
    }
}