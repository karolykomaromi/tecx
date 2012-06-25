namespace TecX.Unity.Test
{
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Groups;
    using TecX.Unity.Test.TestObjects;

    [TestClass]
    public class SemanticGroupFixture
    {
        [TestMethod]
        public void CanGroupRegistrations()
        {
            var container = new UnityContainer();

            container.AddNewExtension<SemanticGroupExtension>();

            container.RegisterGroup(c =>
                {
                    c.RegisterType<IVehicle, Car>("Car");
                    c.RegisterType<IWheel, CarWheel>();
                    c.RegisterType<IEngine, CarEngine>();
                });

            container.RegisterGroup(c =>
                {
                    c.RegisterType<IVehicle, Motorcycle>("Motorcycle");
                    c.RegisterType<IWheel, MotorcycleWheel>();
                    c.RegisterType<IEngine, MotorcycleEngine>();
                });

            var car = container.Resolve<IVehicle>("Car");
            Assert.IsInstanceOfType(car.Wheel, typeof(CarWheel));
            Assert.IsInstanceOfType(car.Engine, typeof(CarEngine));

            var motorcycle = container.Resolve<IVehicle>("Motorcycle");
            Assert.IsInstanceOfType(motorcycle.Wheel, typeof(MotorcycleWheel));
            Assert.IsInstanceOfType(motorcycle.Engine, typeof(MotorcycleEngine));
        }

        [TestMethod]
        public void CanCreateGroupWithOpenGeneric()
        {
            var container = new UnityContainer();

            container.AddNewExtension<SemanticGroupExtension>();

            container.RegisterGroup(c =>
                {
                    c.RegisterType<NeedsCollection>("1");
                    c.RegisterType(typeof(IEnumerable<>), typeof(List<>), new InjectionConstructor());
                });

            var sut = container.Resolve<NeedsCollection>("1");

            Assert.IsInstanceOfType(sut.Collection, typeof(List<string>));
        }
    }
}
