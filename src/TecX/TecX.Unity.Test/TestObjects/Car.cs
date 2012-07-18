namespace TecX.Unity.Test.TestObjects
{
    public class Car : IVehicle
    {
        public IWheel Wheel { get; set; }
        public IEngine Engine { get; set; }
        public Car(IWheel wheel, IEngine engine)
        {
            Wheel = wheel;
            Engine = engine;
        }
    }
}