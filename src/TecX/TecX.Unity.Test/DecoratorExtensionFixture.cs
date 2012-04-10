﻿namespace TecX.Unity.Test
{
    using Microsoft.Practices.Unity;

    using TecX.Common.Extensions.Primitives;
    using TecX.Unity.Decoration;
    using TecX.Unity.Test.TestObjects;

    using Xunit;

    public class DecoratorExtensionFixture
    {
        [Fact]
        public void CreatesTheMostDependentType()
        {
            var container = new UnityContainer()
                .AddExtension(new DecoratorExtension())
                .RegisterType<IContract, ContractDecorator>("1")
                .RegisterType<IContract, Contract>("1");

            var sut = container.Resolve<IContract>("1");
            Assert.NotNull(sut);
            Assert.IsType(typeof(ContractDecorator), sut);
            Assert.IsType(typeof(Contract), ((ContractDecorator)sut).Base);
        }

        [Fact]
        public void UsesSpecialRegistrationFeatures()
        {
            var container = new UnityContainer()
                .AddNewExtension<DecoratorExtension>()
                .RegisterType<IContract, ContractDecorator>(new InjectionProperty("Bar", "2"))
                .RegisterType<IContract, Contract>(new InjectionProperty("Foo", "1"));

            var sut = container.Resolve<IContract>();

            Assert.Equal("2", sut.To<ContractDecorator>().Bar);
            Assert.Equal("1", sut.To<ContractDecorator>().Base.To<Contract>().Foo);
        }

        [Fact]
        public void CanWrapBusinessClass()
        {
            var container = new UnityContainer();

            container.AddNewExtension<DecoratorExtension>();

            container.RegisterType<ICompany, CompanyWrapper2>();
            container.RegisterType<ICompany, CompanyWrapper1>();
            container.RegisterType<ICompany, CompanyImplementation>();

            var company = container.Resolve<ICompany>();

            Assert.Equal("-->Wrapper2-->Wrapper1-->Company", company.Foo());
        }
    }
}
