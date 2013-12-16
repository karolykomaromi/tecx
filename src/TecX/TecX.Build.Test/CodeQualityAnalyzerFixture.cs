namespace TecX.Build.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using StyleCop;

    [TestClass]
    [DeploymentItem(@"Resources\Settings.StyleCop")]
    public class CodeQualityAnalyzerFixture
    {
        private readonly StyleCopConsole console;

        private CodeProject codeProject;

        private ICollection<string> output;

        private ICollection<Violation> violations;

        public CodeQualityAnalyzerFixture()
        {
            string settings = Path.GetFullPath("Settings.StyleCop");

            string[] addinPaths = new string[0];
            this.console = new StyleCopConsole(settings, false, null, addinPaths, true);
            this.console.ViolationEncountered += (s, e) => this.violations.Add(e.Violation);
            this.console.OutputGenerated += (s, e) => this.output.Add(e.Output);
        }

        [TestInitialize]
        public void Setup()
        {
            this.violations = new List<Violation>();

            this.output = new List<string>();

            Configuration configuration = new Configuration(new string[0]);

            this.codeProject = new CodeProject(Guid.NewGuid().GetHashCode(), null, configuration);
        }

        [TestCleanup]
        public void TearDown()
        {
            this.codeProject = null;
        }

        [TestMethod]
        [DeploymentItem(@"Resources\TooManyConstructorArguments.cs")]
        public void Should_Flag_TooManyCtorParameters()
        {
            this.AnalyzeCodeWithAssertion("TooManyConstructorArguments.cs", 1);
        }

        [TestMethod]
        [DeploymentItem(@"Resources\IncorrectRethrow.cs")]
        public void Should_Flag_IncorrectRethrow()
        {
            this.AnalyzeCodeWithAssertion("IncorrectRethrow.cs", 1);
        }

        private void AnalyzeCodeWithAssertion(string codeFileName, int expectedViolations)
        {
            this.AddSourceCode(codeFileName);
            this.StartAnalysis();
            this.WriteViolationsToConsole();
            this.WriteOutputToConsole();
            Assert.AreEqual(expectedViolations, this.violations.Count);
        }

        private void WriteOutputToConsole()
        {
            Console.WriteLine(string.Join(Environment.NewLine, this.output.ToArray()));
        }

        private void WriteViolationsToConsole()
        {
            foreach (var violation in this.violations)
            {
                Console.WriteLine(violation.Message);
            }
        }

        private void AddSourceCode(string fileName)
        {
            string sourceCodeFile = Path.GetFullPath(fileName);

            if (!File.Exists(sourceCodeFile))
            {
                throw new FileNotFoundException(string.Format("Source file '{0}' does not exist.", sourceCodeFile), sourceCodeFile);
            }

            bool sourceCodeSuccessfullyLoaded = this.console.Core.Environment.AddSourceCode(this.codeProject, sourceCodeFile, null);

            if (sourceCodeSuccessfullyLoaded == false)
            {
                throw new InvalidOperationException(string.Format("Source file '{0}' could not be loaded.", sourceCodeFile));
            }
        }

        private void StartAnalysis()
        {
            var projects = new[] { this.codeProject };

            bool result = this.console.Start(projects, true);

            if (result == false)
            {
                throw new InvalidOperationException("StyleCopConsole.Start had a problem.");
            }
        }
    }
}
