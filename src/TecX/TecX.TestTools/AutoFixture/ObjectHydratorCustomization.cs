namespace TecX.TestTools.AutoFixture
{
    using System.Linq;

    using Foundation.ObjectHydrator;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Kernel;

    public class ObjectHydratorCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var builders = from m in new DefaultTypeMap() 
                           select new HydratorAdapter(m);

            fixture.Customizations.Add(new CompositeSpecimenBuilder(builders));
        }
    }
}