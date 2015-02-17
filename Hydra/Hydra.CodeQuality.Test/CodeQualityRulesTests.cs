namespace Hydra.CodeQuality.Test
{
    using System;
    using Hydra.CodeQuality.Rules;
    using Xunit.Extensions;

    public class CodeQualityRulesTests : RuleTests
    {
        [Theory]
        [InlineData("TooManyConstructorArguments.cs", typeof(ConstructorMustNotHaveMoreThanFourParameters))]
        [InlineData("TooManyMethodArguments.cs", typeof(MethodMustNotHaveMoreThanFourParameters))]
        [InlineData("IncorrectRethrow.cs", typeof(IncorrectRethrow))]
        public void Should_Flag_Violation_Of_Rule(string fileWithViolation, Type rule)
        {
            this.AssertCodeViolatesRule(fileWithViolation, rule);
        }
    }
}