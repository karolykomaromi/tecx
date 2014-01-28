namespace Recipe
{
    using System.Runtime.Serialization;
    using System.ServiceModel;

    [ServiceContract]
    public interface IRecipeService
    {
        Recipe[] GetRecipes();

        Recipe GetRecipe(long id);

        Ingredient[] GetIngredients();

        Ingredient GetIngredient(long id);
    }

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

    [DataContract]
    public class Measure
    {
    }

    [DataContract]
    public class VolumeMeasure : Measure
    {
    }

    [DataContract]
    public class WeightMeasure : Measure
    {
    }

    [DataContract]
    public class SpecialKitchenMeasure : Measure
    {
    }

    [DataContract]
    public class Ingredient
    {
        [DataMember]
        public long Id { get; set; }
    }

    public class RecipeService : IRecipeService
    {
        public Recipe[] GetRecipes()
        {
            throw new System.NotImplementedException();
        }

        public Recipe GetRecipe(long id)
        {
            throw new System.NotImplementedException();
        }

        public Ingredient[] GetIngredients()
        {
            throw new System.NotImplementedException();
        }

        public Ingredient GetIngredient(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}
