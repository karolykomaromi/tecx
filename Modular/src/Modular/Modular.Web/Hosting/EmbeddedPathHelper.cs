namespace Modular.Web.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public static class EmbeddedPathHelper
    {
        public static string ToAppRelative(string manifestResourceName)
        {
            Contract.Requires(!string.IsNullOrEmpty(manifestResourceName));
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            int idx = manifestResourceName.IndexOf("Assets.", StringComparison.Ordinal);

            string path = "~/" + manifestResourceName.Substring(idx + 7);

            path = path.Replace('.', '/');

            idx = path.LastIndexOf("/", StringComparison.Ordinal);

            path = path.Remove(idx, 1);

            path = path.Insert(idx, ".");

            return path;
        }

        public static IEnumerable<string> GetDirectories(string manifestResourceName)
        {
            Contract.Requires(!string.IsNullOrEmpty(manifestResourceName));
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

            int idx = manifestResourceName.IndexOf("Assets.", StringComparison.Ordinal);

            string path = manifestResourceName.Substring(idx + 7);

            string[] parts = path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i <= parts.Length - 2; i++)
            {
                string directory = "~/" + string.Join("/", parts.Take(i)) + "/";

                yield return directory;
            }
        }
    }
}