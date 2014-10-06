namespace TecX.Unity.Test.TestObjects
{
    public interface IVehicle
    {
        IWheel Wheel { get; set; }
        IEngine Engine { get; set; }
    }
}