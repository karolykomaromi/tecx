using System;
using Cars.CodeQuality.Rules;
using Xunit;

namespace Cars.CodeQuality.Test
{
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

        [Theory]
        [InlineData(KnownTestFiles.TestFiles.CatchWithTypeOnly, typeof(IncorrectRethrow))]
        [InlineData(KnownTestFiles.TestFiles.EmptyCatchWithoutType, typeof(IncorrectRethrow))]
        public void Should_Not_Flag_Violation_Of_Rule(string fileWithViolation, Type rule)
        {
            this.AssertCodeDoesNotViolateRule(fileWithViolation, rule);
        }
    }
}