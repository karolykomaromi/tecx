namespace Recipe
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Ingredient
    {
        [DataMember]
        public long Id { get; set; }
    }
}