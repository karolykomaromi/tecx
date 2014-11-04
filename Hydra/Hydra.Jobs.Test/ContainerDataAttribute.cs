namespace Hydra.Jobs.Test
{
    using System;
    using AutoMapper;
    using Hydra.Jobs.Transfer;
    using Hydra.TestTools;
    using Microsoft.Practices.Unity;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ContainerDataAttribute : AutoDataAttribute
    {
        public ContainerDataAttribute()
            : base(new Fixture().Customize(
                new ContainerCustomization(
                    new UnityContainer().RegisterType<IMappingEngine>(new InjectionFactory(_ =>
                        {
                            Mapper.AddProfile<JobsProfile>();
                            return Mapper.Engine;
                        })))))
        {
        }
    }
}
