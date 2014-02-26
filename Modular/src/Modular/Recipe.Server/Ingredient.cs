using System.Runtime.Serialization;

namespace Recipe
{
    [DataContract]
    public class Ingredient
    {
        [DataMember]
        public long Id { get; set; }
    }
}