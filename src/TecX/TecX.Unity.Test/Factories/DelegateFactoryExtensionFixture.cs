﻿using System;

namespace TecX.Unity.Test.Factories
{
    using Microsoft.Practices.Unity;
    using TecX.Unity.Factories;
    using TecX.Unity.Test.TestObjects;
    using Xunit;

    public class DelegateFactoryExtensionFixture
    {
        [Fact]
        public void Should_Auto_Resolve_Delegate()
        {
            IUnityContainer container = new UnityContainer().AddNewExtension<DelegateFactoryExtension>();

            container.RegisterType<IRepository, Repository>();

            SomeFactoryDelegate factory = container.Resolve<SomeFactoryDelegate>();

            IRepository repository = factory(true);

            Assert.NotNull(repository);
            Assert.True(repository.IsReadOnly);
        }

        [Fact]
        public void Should_Not_Override_Existing_BuildPlanPolicy()
        {
            bool usedExistingRegistration = false;

            IUnityContainer container = new UnityContainer().AddNewExtension<DelegateFactoryExtension>();
            
            SomeFactoryDelegate expected = readOnly =>
                {
                    usedExistingRegistration = true;
                    return new Repository(readOnly);
                };

            container.RegisterInstance(expected);

            SomeFactoryDelegate actual = container.Resolve<SomeFactoryDelegate>();

            Assert.NotNull(actual(true));

            Assert.True(usedExistingRegistration);
        }

        [Fact]
        public void Should_Resolve_Func_Delegate()
        {
            IUnityContainer container = new UnityContainer().AddNewExtension<DelegateFactoryExtension>();

            var factory = container.Resolve<Func<string, string, string, OrderOfAppearanceMatters>>();

            var sut = factory("1", "2", "3");

            Assert.Equal("1", sut.First);
            Assert.Equal("2", sut.Second);
            Assert.Equal("3", sut.Third);
        }
    }
}
