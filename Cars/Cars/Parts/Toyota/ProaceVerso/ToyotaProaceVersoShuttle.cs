namespace Cars.Parts.Toyota.ProaceVerso
{
    using Cars.I18n;

    public class ToyotaProaceVersoShuttle : PackageBuilder
    {
        public ToyotaProaceVersoShuttle()
        {
            this.PartNumber("SHUTTLE")
                .Abstract(a => a.GermanGermany("Toyota Proace Verso Shuttle"))
                .Part(ToyotaProaceVersoParts.Seats9)
                .Part(ToyotaProaceVersoParts.PassengerBench)
                .Part(ToyotaProaceVersoParts.SlidingDoorPassenger)
                .Part(ToyotaProaceVersoParts.RearDoorWithWindows)
                .Part(ToyotaProaceVersoParts.Airbags)
                .Part(ToyotaProaceVersoParts.FogLight)
                .Part(ToyotaProaceVersoParts.CruiseControl)
                .Part(ToyotaProaceVersoParts.ParkingSensors)
                .Part(ToyotaProaceVersoParts.PowerMirror)
                .Part(ToyotaProaceVersoParts.AirConditionAutomatic)
                .Part(ToyotaProaceVersoParts.Radio)
                .Part(ToyotaProaceVersoParts.PowerDoorLocks)
                .Part(ToyotaProaceVersoParts.SteelWheel16);
        }
    }
}