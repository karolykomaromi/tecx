namespace Hydra.Infrastructure.Reflection
{
    using System.Reflection.Emit;

    public class DecomissioningDecoraptorBuilderContext : BuilderContext
    {
        public FieldBuilder ReleaseField { get; set; }

        public MethodBuilder ReleaseMethod { get; set; }
    }
}