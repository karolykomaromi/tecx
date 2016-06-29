using Cars.Measures;

namespace Cars.Parts.Engines
{
    using System;
    using System.Diagnostics.Contracts;

    public class EngineBuilder : Builder<Engine>
    {
        private PartNumber partNumber;

        private FuelConsumptionBuilder minAveragefuelConsumption;
        private Volume cylinderCapacity;
        private bool hasStartStopSystem;
        private FuelConsumptionBuilder maxAveragefuelConsumption;
        private PowerAtRpmBuilder power;
        private TorqueAtRpmBuilder torque;
        private ExhaustEmissionStandard emission;
        private TransmissionBuilder transmission;
        private WeightPerDistanceBuilder maxExhaust;
        private WeightPerDistanceBuilder minExhaust;
        private ExhaustEfficiencyClass efficiencyClass;

        public EngineBuilder()
        {
            this.partNumber = Cars.Parts.PartNumber.Empty;
            this.minAveragefuelConsumption = new FuelConsumptionBuilder();
            this.maxAveragefuelConsumption = new FuelConsumptionBuilder();
            this.cylinderCapacity = Volume.Zero;
            this.hasStartStopSystem = true;
            this.power = new PowerAtRpmBuilder();
            this.torque = new TorqueAtRpmBuilder();
            this.emission = ExhaustEmissionStandards.Euro6;
            this.maxExhaust = new WeightPerDistanceBuilder();
            this.minExhaust = new WeightPerDistanceBuilder();
            this.efficiencyClass = ExhaustEfficiencyClasses.B;
        }

        public EngineBuilder PartNumber(PartNumber partNumber)
        {
            Contract.Requires(partNumber != null);
            Contract.Ensures(Contract.Result<EngineBuilder>() != null);

            this.partNumber = partNumber;

            return this;
        }

        public EngineBuilder Transmission(Action<TransmissionBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<EngineBuilder>() != null);

            var builder = new TransmissionBuilder();

            action(builder);

            this.transmission = builder;

            return this;
        }

        public EngineBuilder MaxTorque(Action<TorqueAtRpmBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<EngineBuilder>() != null);

            var builder = new TorqueAtRpmBuilder();

            action(builder);

            this.torque = builder;

            return this;
        }

        public EngineBuilder MaxPower(Action<PowerAtRpmBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<EngineBuilder>() != null);

            var builder = new PowerAtRpmBuilder();

            action(builder);

            this.power = builder;

            return this;
        }

        public EngineBuilder MinAverageFuelConsumption(Action<FuelConsumptionBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<EngineBuilder>() != null);

            FuelConsumptionBuilder builder = new FuelConsumptionBuilder();

            action(builder);

            this.minAveragefuelConsumption = builder;

            return this;
        }

        public EngineBuilder MaxAverageFuelConsumption(Action<FuelConsumptionBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<EngineBuilder>() != null);

            FuelConsumptionBuilder builder = new FuelConsumptionBuilder();

            action(builder);

            this.maxAveragefuelConsumption = builder;

            return this;
        }

        public EngineBuilder CylinderCapacity(Volume cylinderCapacity)
        {
            Contract.Requires(cylinderCapacity != null);
            Contract.Ensures(Contract.Result<EngineBuilder>() != null);

            this.cylinderCapacity = cylinderCapacity;

            return this;
        }

        public EngineBuilder StartStopSystem(bool hasStartStopSystem)
        {
            Contract.Ensures(Contract.Result<EngineBuilder>() != null);

            this.hasStartStopSystem = hasStartStopSystem;

            return this;
        }

        public EngineBuilder ExhaustEmissionStandard(ExhaustEmissionStandard emission)
        {
            Contract.Requires(emission != null);
            Contract.Ensures(Contract.Result<EngineBuilder>() != null);

            this.emission = emission;

            return this;
        }

        public EngineBuilder MaxExhaust(Action<WeightPerDistanceBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<EngineBuilder>() != null);

            var builder = new WeightPerDistanceBuilder();

            action(builder);

            this.maxExhaust = builder;

            return this;
        }

        public EngineBuilder MinExhaust(Action<WeightPerDistanceBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<EngineBuilder>() != null);

            var builder = new WeightPerDistanceBuilder();

            action(builder);

            this.minExhaust = builder;

            return this;
        }

        public EngineBuilder ExhaustEfficiencyClass(ExhaustEfficiencyClass efficiencyClass)
        {
            Contract.Requires(efficiencyClass != null);
            Contract.Ensures(Contract.Result<EngineBuilder>() != null);

            this.efficiencyClass = efficiencyClass;

            return this;
        }

        public override Engine Build()
        {
            return new Engine(this.partNumber);
        }
    }
}