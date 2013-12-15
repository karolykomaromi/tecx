namespace TecX.Unity.Test
{
    using Microsoft.Practices.Unity;

    using TecX.Unity.Enums;

    using Xunit;

    public class EnumFixture
    {
        [Fact]
        public void CanResolveDefaultEnumValueForInjection()
        {
            var container = new UnityContainer().AddNewExtension<EnumExtension>();

            var sut = container.Resolve<DependsOnEnumBasedOnInt>();

            Assert.Equal(EnumBasedOnInt.Default, sut.Enum);
        }

        [Fact]
        public void CanResolveDefaultEnumValueForByteBasedEnum()
        {
            var container = new UnityContainer().AddNewExtension<EnumExtension>();

            var sut = container.Resolve<DependsOnEnumBasedOnByte>();

            Assert.Equal(EnumBasedOnByte.Default, sut.Enum);
        }

        [Fact]
        public void CantResolveEnumsWithoutExtension()
        {
            var container = new UnityContainer();

            Assert.Throws<ResolutionFailedException>(() => container.Resolve<DependsOnEnumBasedOnByte>());
        }
    }

    internal enum EnumBasedOnByte
    {
        Default = 0,
        NonDefault = 1
    }

    internal class DependsOnEnumBasedOnByte
    {
        public DependsOnEnumBasedOnByte(EnumBasedOnByte e)
        {
            this.Enum = e;
        }

        public EnumBasedOnByte Enum { get; set; }
    }

    internal class DependsOnEnumBasedOnInt
    {
        public DependsOnEnumBasedOnInt(EnumBasedOnInt e)
        {
            this.Enum = e;
        }

        public EnumBasedOnInt Enum { get; set; }
    }

    internal enum EnumBasedOnInt
    {
        Default = 0,
        NonDefault = 1
    }
}
