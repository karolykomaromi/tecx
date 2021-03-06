﻿namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Test.TestObjects;
    using TecX.Unity.Injection;

    [TestClass]
    public class When_RegisteringCtorWithParameterConvention : Given_ContainerAndBuilder
    {
        private HasCtorWithParameterConvention sut;

        protected override void Arrange()
        {
            base.Arrange();

            this.builder.For<HasCtorWithParameterConvention>().Use<HasCtorWithParameterConvention>().Ctor(new ConstructorParameter(new Foo()));
        }

        protected override void Act()
        {
            base.Act();

            this.sut = this.container.Resolve<HasCtorWithParameterConvention>();
        }

        [TestMethod]
        public void Then_AppliesCtorConvention()
        {
            Assert.IsNotNull(this.sut.Foo);
        }
    }
}
