namespace Recipe.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Ingredient
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}