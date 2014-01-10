namespace TecX.Build.Test
{
    using System;
    using System.Collections.Generic;

    using StyleCop;
    using StyleCop.CSharp;

    using Xunit;

    public class CodeQualityAnalyzerFixture
    {
        private readonly StyleCopObjectConsole console;
        private readonly ICollection<string> output;
        private readonly ICollection<Violation> violations;

        private CodeProject codeProject;

        public CodeQualityAnalyzerFixture()
        {
            this.violations = new List<Violation>();
            this.output = new List<string>();

            ICollection<string> addinPaths = new[] { "." };

            ObjectBasedEnvironment environment = new ObjectBasedEnvironment(ResourceBasedSourceCode.Create, GetProjectSettings);

            this.console = new StyleCopObjectConsole(environment, null, addinPaths, true);

            CsParser parser = new CsParser();
            parser.FileTypes.Add("CS");

            this.console.Core.Environment.AddParser(parser);
            this.console.Core.ViolationEncountered += (s, e) => this.violations.Add(e.Violation);
            this.console.Core.OutputGenerated += (s, e) => this.output.Add(e.Output);

            this.codeProject = new CodeProject(0, null, new Configuration(null));
        }

        [Fact]
        public void Should_Flag_TooManyCtorParameters()
        {
            this.AnalyzeCodeWithAssertion("TooManyConstructorArguments.cs", 1);
        }

        [Fact]
        public void Should_Flag_IncorrectRethrow()
        {
            this.AnalyzeCodeWithAssertion("IncorrectRethrow.cs", 1);
        }
        
        private Settings GetProjectSettings(string path, bool readOnly)
        {
            return null;
        }

        private void AnalyzeCodeWithAssertion(string codeFileName, int expectedViolations)
        {
            this.AddSourceCode(codeFileName);
            this.StartAnalysis();
            this.WriteViolationsToConsole();
            this.WriteOutputToConsole();

            Assert.Equal(expectedViolations, this.violations.Count);
        }

        private void WriteOutputToConsole()
        {
            foreach (var o in this.output)
            {
                Console.WriteLine(o);
            }
        }

        private void WriteViolationsToConsole()
        {
            foreach (var violation in this.violations)
            {
                Console.WriteLine(violation.Message);
            }
        }

        private void AddSourceCode(string resourceFileName)
        {
            bool sourceCodeSuccessfullyLoaded = this.console.Core.Environment.AddSourceCode(this.codeProject, resourceFileName, null);

            if (sourceCodeSuccessfullyLoaded == false)
            {
                throw new InvalidOperationException(string.Format("Source file '{0}' could not be added.", resourceFileName));
            }
        }

        private void StartAnalysis()
        {
            IList<CodeProject> projects = new[] { this.codeProject };

            this.console.Start(projects);
        }
    }
}
