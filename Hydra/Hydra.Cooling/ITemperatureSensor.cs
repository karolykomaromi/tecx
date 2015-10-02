namespace Hydra.Cooling
{
    public interface ITemperatureSensor
    {
        Temperature CurrentTemperature { get; }
    }

    public interface IThermostat
    {
        void SetTargetTemperature(Temperature temperature);
    }
}
