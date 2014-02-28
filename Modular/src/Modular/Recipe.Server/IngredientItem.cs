namespace Recipe
{
    using System.Runtime.Serialization;

    [DataContract]
    [KnownType(typeof(VolumeMeasure))]
    [KnownType(typeof(WeightMeasure))]
    [KnownType(typeof(SpecialKitchenMeasure))]
    public class IngredientItem
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public Ingredient Ingredient { get; set; }

        [DataMember]
        public Measure Quantity { get; set; }
    }
}