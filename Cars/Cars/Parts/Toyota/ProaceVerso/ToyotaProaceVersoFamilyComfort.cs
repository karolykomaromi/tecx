namespace Cars.Parts.Toyota.ProaceVerso
{
    using Cars.I18n;

    public class ToyotaFamilyComfort : PackageBuilder
    {
        public ToyotaFamilyComfort()
        {
            this.PartNumber("FAMILY_COMFORT")
                .Abstract(a => a.GermanGermany("Toyota Proace Verso Family Comfort"))
                .Part(ToyotaProaceVersoParts.Seats8)
                .Part(ToyotaProaceVersoParts.IndividualSeats)
                .Part(ToyotaProaceVersoParts.RemovableSeats)
                .Part(ToyotaProaceVersoParts.SeatHeating)
                .Part(ToyotaProaceVersoParts.SlidingDoorPassenger)
                .Part(ToyotaProaceVersoParts.SlidingDoorDriver)
                .Part(ToyotaProaceVersoParts.RearDoorWithWindows)
                .Part(ToyotaProaceVersoParts.RearWindow)
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
                .Part(ToyotaProaceVersoParts.FoldingTables)
                .Part(ToyotaProaceVersoParts.AlloyWheel17);
        }
    }
}