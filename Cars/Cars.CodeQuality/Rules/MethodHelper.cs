namespace Cars.CodeQuality.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using StyleCop.CSharp;

    public static class MethodHelper
    {
        public static string Signature(Method method)
        {
            string signature = method == null 
                ? string.Empty
                : (method.FullNamespaceName ?? string.Empty).Replace("Root.", string.Empty) + 
                  "(" + 
                  string.Join(", ", (method.Parameters ?? new List<Parameter>()).Select(p => p.Type + " " + p.Name)) +
                  ")";

            return signature;
        }
    }
}