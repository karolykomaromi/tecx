namespace TecX.Unity.Injection
{
    using System;
    using System.Collections.Generic;

    using TecX.Common;

    public class ByTypeNamingConvention : NamingConvention
    {
        private readonly HashSet<string> snippets;

        public ByTypeNamingConvention(Type type)
        {
            Guard.AssertNotNull(type, "type");

            this.snippets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            InitializeSnippets(type, this.snippets);
        }

        public override bool NameMatches(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            return this.snippets.Contains(name);
        }

        private static void InitializeSnippets(Type type, HashSet<string> snippets)
        {
            string typeName = type.Name;

            if (typeName.StartsWith("i", StringComparison.OrdinalIgnoreCase))
            {
                typeName = typeName.Substring(1);
            }

            for (int i = 0; i < typeName.Length; i++)
            {
                char c = typeName[i];

                if (char.IsUpper(c))
                {
                    string snippet = typeName.Substring(i);
                    snippets.Add(snippet);
                }
            }
        }
    }
}