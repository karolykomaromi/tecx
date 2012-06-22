namespace TecX.Unity.Groups
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public interface ISemanticGroup
    {
        string Name { get; }

        ISemanticGroup With(Type from, Type to, string name, params InjectionMember[] injectionMembers);
    }

    public static class SemanticGroupExtensions
    {
        public static ISemanticGroup With(this ISemanticGroup semanticGroup, Type from, Type to, params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(semanticGroup, "semanticGroup");
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");

            return semanticGroup.With(from, to, semanticGroup.Name, injectionMembers);
        }

        public static ISemanticGroup With<TFrom, TTo>(this ISemanticGroup semanticGroup)
        {
            return With<TFrom, TTo>(semanticGroup, semanticGroup.Name);
        }

        public static ISemanticGroup With<TFrom, TTo>(this ISemanticGroup semanticGroup, string name, params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(semanticGroup, "semanticGroup");
            Guard.AssertNotEmpty(name, "name");

            return semanticGroup.With(typeof(TFrom), typeof(TTo), name, injectionMembers);
        }
    }
}