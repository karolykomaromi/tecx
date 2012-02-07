namespace TecX.Unity.Configuration.Expressions
{
    using System;

    internal interface IExtensibleConfiguration
    {
        void AddAlternation(Action<RegistrationFamily> action);
    }
}