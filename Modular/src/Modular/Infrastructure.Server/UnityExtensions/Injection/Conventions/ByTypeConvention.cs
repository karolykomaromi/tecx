namespace Infrastructure.UnityExtensions.Injection.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.Practices.Unity;

    public class ByTypeConvention : IParameterMatchingConvention
    {
        private readonly Dictionary<Type, ICollection<string>> snippetsByType;

        public ByTypeConvention()
        {
            this.snippetsByType = new Dictionary<Type, ICollection<string>>();
        }

        public bool IsMatch(Parameter argument, ParameterInfo parameter)
        {
            ResolvedParameter rp = argument.Value as ResolvedParameter;

            Type type = rp != null ? rp.ParameterType : argument.Value.GetType();

            if (parameter.ParameterType.IsAssignableFrom(type) &&
                !FileNameConvention.HintsAtFileName(argument.Name) &&
                !ConnectionStringConvention.HintsAtConnectionString(argument.Name))
            {
                return true;
            }

            ICollection<string> snippets;
            if (!this.snippetsByType.TryGetValue(type, out snippets))
            {
                snippets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                InitializeSnippets(type, snippets);

                this.snippetsByType.Add(type, snippets);
            }

            return snippets.Contains(parameter.Name);
        }

        private static void InitializeSnippets(Type type, ICollection<string> snippets)
        {
            var allBaseClassesAndInterfaces = TypeHelper.GetAllBaseClassesAndInterfaces(type);

            foreach (var t in allBaseClassesAndInterfaces)
            {
                string typeName = t.Name;

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
}