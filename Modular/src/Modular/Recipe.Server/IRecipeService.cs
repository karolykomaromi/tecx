namespace Recipe
{
    using System.ServiceModel;
    using Recipe.Entities;

    [ServiceContract]
    public interface IRecipeService
    {
        [OperationContract]
        Recipe[] GetRecipes(int skip, int take);

        [OperationContract]
        Recipe GetRecipe(long id);

        [OperationContract]
        Ingredient[] GetIngredients(int skip, int take);

        [OperationContract]
        Ingredient GetIngredient(long id);
    }
}