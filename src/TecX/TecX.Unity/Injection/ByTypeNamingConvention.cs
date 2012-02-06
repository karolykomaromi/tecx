namespace TecX.Unity.Injection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TecX.Common;

    public class ByTypeNamingConvention : NamingConvention
    {
        private readonly Type type;

        private readonly List<string> snippets;

        public ByTypeNamingConvention(Type type)
        {
            Guard.AssertNotNull(type, "type");
            this.type = type;
            this.snippets = new List<string>();
            this.InitializeSnippets();
        }

        protected override bool NameMatchesCore(string name)
        {
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