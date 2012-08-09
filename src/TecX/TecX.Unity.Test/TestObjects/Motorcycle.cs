namespace TecX.Unity.Test.TestObjects
{
    public class Motorcycle : IVehicle
    {
        public IWheel Wheel { get; set; }
        public IEngine Engine { get; set; }
        public Motorcycle(IWheel wheel, IEngine engine)
        {
            Wheel = wheel;
            Engine = engine;
        }
    }
}