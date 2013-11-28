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
            this.console.ViolationEncountered += (sender, args) => violations.Add(args.Violation);
            this.console.OutputGenerated += (sender, args) => output.Add(args.Output);
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
            Assert.AreEqual(expectedViolations, violations.Count);
        }

        private void WriteOutputToConsole()
        {
            Console.WriteLine(string.Join(Environment.NewLine, output.ToArray()));
        }

        private void WriteViolationsToConsole()
        {
            foreach (var violation in violations)
            {
                Console.WriteLine(violation.Message);
            }
        }

        private void AddSourceCode(string fileName)
        {
            fileName = Path.GetFullPath(fileName);
            bool result = this.console.Core.Environment.AddSourceCode(codeProject, fileName, null);
            if (result == false)
            {
                throw new ArgumentException("Source file could not be loaded.", fileName);
            }
        }

        private void StartAnalysis()
        {
            var projects = new[] { codeProject };
            bool result = this.console.Start(projects, true);
            if (result == false)
            {
                throw new ArgumentException("StyleCopConsole.Start had a problem.");
            }
        }
    }
}
