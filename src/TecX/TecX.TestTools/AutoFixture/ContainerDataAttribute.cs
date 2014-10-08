using System;
using System.Text;

using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace TecX.TestTools.AutoFixture
{
    using Microsoft.Practices.Unity;

    public class ContainerDataAttribute : AutoDataAttribute
    {
        public ContainerDataAttribute()
            : base(new Fixture().Customize(new ContainerCustomization(new UnityContainer().AddExtension(new FindContainerConfigurations()))))
        {
        }
    }
}
