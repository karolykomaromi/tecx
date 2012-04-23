namespace TecX.Unity.Mixin.Test.TestObjects
{
    using Remotion.Mixins;

    public class ToStringMixin
    {
        [OverrideTarget]
        public new string ToString()
        {
            return "2";
        }
    }
}