namespace Cars.Parts.Toyota.ProaceVerso
{
    using Cars.I18n;

    public class ToyotaProaceVersoShuttleComfort : PackageBuilder
    {
        public ToyotaProaceVersoShuttleComfort()
        {
            this.PartNumber("SHUTTLE_COMFORT")
                .Abstract(a => a.GermanGermany("Toyota Proace Verso Shuttle Comfort"))
                .Part(ToyotaProaceVersoParts.Seats8)
                .Part(ToyotaProaceVersoParts.IndividualSeats)
                .Part(ToyotaProaceVersoParts.SeatHeating)
                .Part(ToyotaProaceVersoParts.SlidingDoorPassenger)
                .Part(ToyotaProaceVersoParts.RearDoorWithWindows)
                .Part(ToyotaProaceVersoParts.Airbags)
                .Part(ToyotaProaceVersoParts.FogLight)
                .Part(ToyotaProaceVersoParts.HeadLightCleaner)
                .Part(ToyotaProaceVersoParts.CruiseControl)
                .Part(ToyotaProaceVersoParts.ParkingSensors)
                .Part(ToyotaProaceVersoParts.PowerMirror)
                .Part(ToyotaProaceVersoParts.PowerMirrorFoldIn)
                .Part(ToyotaProaceVersoParts.AirConditionAutomatic)
                .Part(ToyotaProaceVersoParts.Radio)
                .Part(ToyotaProaceVersoParts.PowerDoorLocks)
                .Part(ToyotaProaceVersoParts.SmarkKey)
                .Part(ToyotaProaceVersoParts.RearViewCamera)
                .Part(ToyotaProaceVersoParts.AlloyWheel17);
        }
    }
}