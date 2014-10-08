namespace Recipe.Entities
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Recipe
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public IngredientItem[] Ingredients { get; set; }
    }
}