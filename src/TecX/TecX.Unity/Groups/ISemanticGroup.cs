namespace TecX.Unity.Groups
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public interface ISemanticGroup
    {
        string Name { get; }

        ISemanticGroup Use(Type from, Type to, string name, params InjectionMember[] injectionMembers);
    }

    public static class SemanticGroupExtensions
    {
        public static ISemanticGroup Use(this ISemanticGroup semanticGroup, Type from, Type to, params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(semanticGroup, "semanticGroup");
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");

            return semanticGroup.Use(from, to, semanticGroup.Name, injectionMembers);
        }

        public static ISemanticGroup Use<TFrom, TTo>(this ISemanticGroup semanticGroup)
        {
            return Use<TFrom, TTo>(semanticGroup, semanticGroup.Name);
        }

        public static ISemanticGroup Use<TFrom, TTo>(this ISemanticGroup semanticGroup, string name, params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(semanticGroup, "semanticGroup");
            Guard.AssertNotEmpty(name, "name");

            return semanticGroup.Use(typeof(TFrom), typeof(TTo), name, injectionMembers);
        }
    }
}