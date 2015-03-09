namespace Hydra.CodeQuality.Test
{
    using System;
    using Hydra.CodeQuality.Rules;
    using Xunit.Extensions;

    public class CodeQualityRulesTests : RuleTests
    {
        [Theory]
        [InlineData(KnownTestFiles.TestFiles.TooManyConstructorArguments, typeof(ConstructorMustNotHaveMoreThanFourParameters))]
        [InlineData(KnownTestFiles.TestFiles.TooManyMethodArguments, typeof(MethodMustNotHaveMoreThanFourParameters))]
        [InlineData(KnownTestFiles.TestFiles.IncorrectRethrow, typeof(IncorrectRethrow))]
        [InlineData(KnownTestFiles.TestFiles.ReturnsDefaultInt, typeof(DontUseDefaultOperator))]
        [InlineData(KnownTestFiles.TestFiles.AsyncVoid, typeof(AsyncMethodsMustReturnTask))]
        [InlineData(KnownTestFiles.TestFiles.SetOnlyProperty, typeof(DontDeclareSetOnlyProperties))]
        public void Should_Flag_Violation_Of_Rule(string fileWithViolation, Type rule)
        {
            this.AssertCodeViolatesRule(fileWithViolation, rule);
        }
    }
}