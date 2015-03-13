namespace Hydra.Jobs.Scripts
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Hydra.Infrastructure;

    public class QuartzTableCreationScripts : Enumeration<QuartzTableCreationScripts>
    {
        public static readonly QuartzTableCreationScripts MySql = new QuartzTableCreationScripts();

        public static readonly QuartzTableCreationScripts Oracle = new QuartzTableCreationScripts();

        public static readonly QuartzTableCreationScripts PostgreSql = new QuartzTableCreationScripts();

        public static readonly QuartzTableCreationScripts Sqlite = new QuartzTableCreationScripts();

        public static readonly QuartzTableCreationScripts SqlServer = new QuartzTableCreationScripts();

        public static readonly QuartzTableCreationScripts SqlServerCe = new QuartzTableCreationScripts();

        private readonly Lazy<string> script;

        public QuartzTableCreationScripts([CallerMemberName]string name = "", [CallerLineNumber]int sortKey = 0)
            : base(name, sortKey)
        {
            this.script = new Lazy<string>(this.GetScript);
        }

        public string Script
        {
            get { return this.script.Value; }
        }

        private string GetScript()
        {
            Assembly assembly = this.GetType().Assembly;
            string scriptName = assembly.GetManifestResourceNames().FirstOrDefault(name => name.EndsWith(this.Name + ".sql", StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(scriptName))
            {
                return string.Empty;
            }

            using (Stream stream = assembly.GetManifestResourceStream(scriptName) ?? Stream.Null)
            {
                using (TextReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string s = reader.ReadToEnd();

                    return s;
                }
            }
        }

        [ContractInvariantMethod]
        private void ObjectInveriant()
        {
            Contract.Invariant(this.script != null);
        }
    }
}
