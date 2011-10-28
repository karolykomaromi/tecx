using Remotion.Mixins;

namespace TecX.Unity.Mixin.Test.TestObjects
{
    public class ToStringMixin
    {
        [OverrideTarget]
        public new string ToString()
        {
            return "2";
        }
    }
}