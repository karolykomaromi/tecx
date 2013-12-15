namespace TecX.Unity.Enums
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class EnumPolicy : IBuildPlanPolicy
    {
        private readonly Type enumType;

        public EnumPolicy(Type enumType)
        {
            Guard.AssertNotNull(enumType, "enumType");

            this.enumType = enumType;
        }

        public void BuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            if (context.Existing == null)
            {
                context.Existing = Enum.ToObject(this.enumType, 0);
            }
        }
    }
}