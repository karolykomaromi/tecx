namespace Hydra.Infrastructure.Reflection
{
    using System.Reflection.Emit;

    public class BuilderContext
    {
        public TypeBuilder TypeBuilder { get; set; }

        public FieldBuilder TargetField { get; set; }

        public MethodBuilder TargetGetter { get; set; }
    }
}