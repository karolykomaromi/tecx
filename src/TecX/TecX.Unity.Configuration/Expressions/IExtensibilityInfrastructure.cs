namespace TecX.Unity.Configuration.Expressions
{
    using System;

    public interface IExtensibilityInfrastructure
    {
        void AddAlternation(CreateRegistrationFamilyExpression expression, Action<RegistrationFamily> action);

        void SetCompilationStrategy(Func<Registration> compilationStrategy);
    }
}