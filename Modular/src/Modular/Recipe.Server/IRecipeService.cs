using System.ServiceModel;

namespace Recipe
{
    [ServiceContract]
    public interface IRecipeService
    {
        Recipe[] GetRecipes();

        Recipe GetRecipe(long id);

        Ingredient[] GetIngredients();

        Ingredient GetIngredient(long id);
    }
}