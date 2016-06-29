namespace Cars.Parts.Toyota.ProaceVerso
{
    using Cars.I18n;

    public class ToyotaProaceVersoFamily : PackageBuilder
    {
        public ToyotaProaceVersoFamily()
        {
            this.PartNumber("FAMILY")
                .Abstract(a => a.GermanGermany("Toyota Proace Verso Family"))
                .Part(ToyotaProaceVersoParts.Seats8)
                .Part(ToyotaProaceVersoParts.IndividualSeats)
                .Part(ToyotaProaceVersoParts.RemovableSeats)
                .Part(ToyotaProaceVersoParts.SlidingDoorPassenger)
                .Part(ToyotaProaceVersoParts.SlidingDoorDriver)
                .Part(ToyotaProaceVersoParts.RearDoorWithWindows)
                .Part(ToyotaProaceVersoParts.RearWindow)
                .Part(ToyotaProaceVersoParts.Airbags)
                .Part(ToyotaProaceVersoParts.FogLight)
                .Part(ToyotaProaceVersoParts.CruiseControl)
                .Part(ToyotaProaceVersoParts.ParkingSensors)
                .Part(ToyotaProaceVersoParts.PowerMirror)
                .Part(ToyotaProaceVersoParts.AirConditionAutomatic)
                .Part(ToyotaProaceVersoParts.Radio)
                .Part(ToyotaProaceVersoParts.PowerDoorLocks)
                .Part(ToyotaProaceVersoParts.FoldingTables)
                .Part(ToyotaProaceVersoParts.SteelWheel17);
        }
    }
}