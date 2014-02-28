namespace Recipe
{
    using System.ServiceModel;

    [ServiceContract]
    public interface IRecipeService
    {
        Recipe[] GetRecipes();

        Recipe GetRecipe(long id);

        Ingredient[] GetIngredients();

        Ingredient GetIngredient(long id);
    }
}