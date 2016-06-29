namespace Cars.Test.Parts
{
    using Cars.Parts;
    using Cars.Parts.Toyota.ProaceVerso;
    using Xunit;

    public class PackageBuilderTests
    {
        [Fact]
        public void Should_Build_Toyota_Configurations()
        {
            Package toyotaShuttle = new ToyotaProaceVersoShuttle();

            Package toyotaShuttleComfort = new ToyotaProaceVersoShuttleComfort();

            Package toyotaFamily = new ToyotaProaceVersoFamily();

            Package toyotaFamilyComfort = new ToyotaFamilyComfort();
        }
    }
}
