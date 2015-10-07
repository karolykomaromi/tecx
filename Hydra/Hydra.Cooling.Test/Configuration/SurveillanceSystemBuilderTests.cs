namespace Hydra.Cooling.Test.Configuration
{
    using System;
    using System.IO.Ports;
    using System.Net;
    using Hydra.Cooling.Sensors;
    using Hydra.Infrastructure;
    using Xunit;

    public class SurveillanceSystemBuilderTests
    {
        [Fact]
        public void Should_Build_SurveillanceSystem()
        {
            var builder = new SurveillanceSystemBuilder();

            builder
                .TemperatureSensor(
                    x =>
                    {
                        x.WithLocalUsbConnection();
                        x.AlertOnTemperatureTooHigh(Celsius.WaterFreezing);
                    })
                .TemperatureSensor(
                    x =>
                    {
                        x.WithNetworkConnection(IPAddress.Loopback);
                        x.AlertOnTemperatureTooLow(Kelvin.AbsoluteZero);
                    })
                .DoorSensor(
                    x =>
                    {
                        x.WithNetworkConnection(IPAddress.Loopback);
                        x.AlertOnDoorOpened();
                    })
                .DoorSensor(
                    x =>
                    {
                        x.WithLocalUsbConnection();
                        x.AlertOnDoorClosed();
                    })
                .EmergencySwitchSensor(
                    x =>
                    {
                        x.WithLocalUsbConnection();
                        x.AlertOnSwitchPressed();
                    })
                .EmergencySwitchSensor(
                    x =>
                    {
                        x.WithNetworkConnection(IPAddress.Loopback);
                        x.AlertOnSwitchReleased();
                    });
        }
    }

    public class SurveillanceSystemBuilder : Builder<SurveillanceSystem>
    {
        public override SurveillanceSystem Build()
        {
            return new SurveillanceSystem();
        }

        public SurveillanceSystemBuilder TemperatureSensor(Action<TemperatureSensorBuilder> action)
        {
            var builder = new TemperatureSensorBuilder();

            action(builder);

            return this;
        }

        public SurveillanceSystemBuilder DoorSensor(Action<DoorSensorBuilder> action)
        {
            var builder = new DoorSensorBuilder();

            action(builder);

            return this;
        }

        public SurveillanceSystemBuilder EmergencySwitchSensor(Action<EmergencySwitchSensorBuilder> action)
        {
            var builder = new EmergencySwitchSensorBuilder();

            action(builder);

            return this;
        }
    }

    public class EmergencySwitchSensorBuilder : SensorBuilder<EmergencySwitchSensorBuilder, IEmergencySwitchSensor>
    {
        public override IEmergencySwitchSensor Build()
        {
            return EmergencySwitchSensor.Null;
        }

        public EmergencySwitchSensorBuilder AlertOnSwitchPressed()
        {
            return this;
        }

        public EmergencySwitchSensorBuilder AlertOnSwitchReleased()
        {
            return this;
        }
    }

    public abstract class SensorBuilder<TBuilder, TSensor> : Builder<TSensor>
        where TBuilder : SensorBuilder<TBuilder, TSensor>
        where TSensor : class 
    {
        public TBuilder WithNetworkConnection(IPAddress address)
        {
            return this as TBuilder;
        }

        public TBuilder WithLocalUsbConnection()
        {
            return this as TBuilder;
        }
    }

    public class DoorSensorBuilder : SensorBuilder<DoorSensorBuilder, IDoorSensor>
    {
        public override IDoorSensor Build()
        {
            return DoorSensor.Null;
        }

        public DoorSensorBuilder AlertOnDoorOpened()
        {
            return this;
        }

        public DoorSensorBuilder AlertOnDoorClosed()
        {
            return this;
        }
    }

    public class TemperatureSensorBuilder : SensorBuilder<TemperatureSensorBuilder, ITemperatureSensor>
    {
        public override ITemperatureSensor Build()
        {
            return TemperatureSensor.Null;
        }

        public TemperatureSensorBuilder AlertOnTemperatureTooHigh(Temperature maxAllowedTemperature)
        {
            return this;
        }

        public TemperatureSensorBuilder AlertOnTemperatureTooLow(Temperature minAllowedTemperature)
        {
            return this;
        }
    }

    public class SurveillanceSystem : IDisposable
    {
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}
