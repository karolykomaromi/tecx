﻿using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.Groups;
using TecX.Unity.Test.TestObjects;

namespace TecX.Unity.Test
{
    [TestClass]
    public class SemanticGroupFixture
    {
        [TestMethod]
        public void CanGroupRegistrations()
        {
            var container = new UnityContainer();

            container.AddNewExtension<SemanticGroupExtension>();

            container.RegisterGroup<IVehicle, Car>("Car").Use<IWheel, CarWheel>().Use<IEngine, CarEngine>();
            container.RegisterGroup<IVehicle, Motorcycle>("Motorcycle").Use<IWheel, MotorcycleWheel>().Use<IEngine, MotorcycleEngine>();

            var car = container.Resolve<IVehicle>("Car");
            Assert.IsInstanceOfType(car.Wheel, typeof(CarWheel));
            Assert.IsInstanceOfType(car.Engine, typeof(CarEngine));

            var motorcycle = container.Resolve<IVehicle>("Motorcycle");
            Assert.IsInstanceOfType(motorcycle.Wheel, typeof(MotorcycleWheel));
            Assert.IsInstanceOfType(motorcycle.Engine, typeof(MotorcycleEngine));
        }
    }
}
