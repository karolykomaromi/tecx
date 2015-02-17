namespace Hydra.CodeQuality.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hydra.CodeQuality.Test.Properties;
    using StyleCop;
    using StyleCop.CSharp;
    using Xunit;

    public abstract class RuleTests
    {
        private readonly StyleCopObjectConsole console;
        private readonly ICollection<string> output;
        private readonly ICollection<Violation> violations;
        private readonly CodeProject codeProject;

        protected RuleTests()
        {
            this.violations = new List<Violation>();
            this.output = new List<string>();

            ICollection<string> addinPaths = new[] { "." };

            ObjectBasedEnvironment environment = new ObjectBasedEnvironment(ResourceBasedSourceCode.Create, RuleTests.GetEmptyProjectSettings);

            this.console = new StyleCopObjectConsole(environment, null, addinPaths, true);

            CsParser parser = new CsParser();
            parser.FileTypes.Add("CS");

            this.console.Core.Environment.AddParser(parser);
            this.console.Core.ViolationEncountered += (s, e) => this.violations.Add(e.Violation);
            this.console.Core.OutputGenerated += (s, e) => this.output.Add(e.Output);

            this.codeProject = new CodeProject(0, null, new Configuration(null));
        }

        protected void AssertCodeViolatesRule(string codeFileName, Type rule)
        {
            this.AddSourceCode(codeFileName);
            this.StartAnalysis();
            this.WriteViolationsToConsole();

            Violation violation = this.violations.FirstOrDefault(v => string.Equals(rule.Name, v.Rule.Name, StringComparison.OrdinalIgnoreCase));

            try
            {
                Assert.NotNull(violation);
            }
            catch
            {
                Console.WriteLine(Resources.RuleNotViolated, rule.Name);

                throw;
            }
        }

        private static Settings GetEmptyProjectSettings(string path, bool readOnly)
        {
            return null;
        }

        private void WriteViolationsToConsole()
        {
            foreach (var violation in this.violations)
            {
                CodeLocation location = violation.Location.GetValueOrDefault(CodeLocation.Empty);

                Console.WriteLine(
                    Resources.ViolationsMessage,
                    violation.Rule.CheckId,
                    violation.Rule.Name,
                    location.LineNumber,
                    location.StartPoint.IndexOnLine,
                    violation.Message);
            }
        }

        private void AddSourceCode(string resourceFileName)
        {
            bool sourceCodeSuccessfullyLoaded = this.console.Core.Environment.AddSourceCode(this.codeProject, resourceFileName, null);

            if (sourceCodeSuccessfullyLoaded == false)
            {
                throw new InvalidOperationException(string.Format(Resources.SourceFileCouldNotBeAdded, resourceFileName));
            }
        }

        private void StartAnalysis()
        {
            IList<CodeProject> projects = new[] { this.codeProject };

            this.console.Start(projects);
        }
    }
}
