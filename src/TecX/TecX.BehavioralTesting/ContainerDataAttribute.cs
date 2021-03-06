﻿namespace TecX.BehavioralTesting
{
    using Microsoft.Practices.Unity;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;

    public class ContainerDataAttribute : AutoDataAttribute
    {
        public ContainerDataAttribute()
            : base(new Fixture().Customize(new ContainerCustomization(new UnityContainer().AddExtension(new ContainerConfiguration()))))
        {
        }
    }
}