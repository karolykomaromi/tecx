namespace Hydra.CodeQuality.Rules
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using StyleCop;

    public abstract class Visitor
    {
        private readonly SourceAnalyzer sourceAnalyzer;

        private readonly Lazy<string> checkId;

        protected Visitor(SourceAnalyzer sourceAnalyzer)
        {
            Contract.Requires(sourceAnalyzer != null);

            this.sourceAnalyzer = sourceAnalyzer;
            this.checkId = new Lazy<string>(this.GetCheckId);
        }

        public SourceAnalyzer SourceAnalyzer
        {
            get { return this.sourceAnalyzer; }
        }

        protected virtual string RuleName
        {
            get { return this.GetType().Name; }
        }

        protected virtual string CheckId
        {
            get { return this.checkId.Value; }
        }

        protected virtual string GetCheckId()
        {
            Type type = this.SourceAnalyzer.GetType();

            string xmlFileName = type.Name + ".xml";

            Assembly assembly = type.Assembly;

            string resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(rn => rn.EndsWith(xmlFileName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(resourceName))
            {
                XDocument xml = XDocument.Load(assembly.GetManifestResourceStream(resourceName));

                XElement rule = xml.Root.Elements("Rule").FirstOrDefault(r =>
                {
                    XAttribute name = r.Attribute("Name");

                    if (name != null && 
                        string.Equals(this.RuleName, name.Value, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    return false;
                });

                XAttribute cid;
                if (rule != null &&
                    (cid = rule.Attribute("CheckId")) != null && 
                    !string.IsNullOrWhiteSpace(cid.Value))
                {
                    return cid.Value;
                }
            }

            return string.Empty;
        }
    }
}