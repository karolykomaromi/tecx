namespace Recipe
{
    using Recipe.Entities;

    public class RecipeService : IRecipeService
    {
        public Recipe[] GetRecipes(int skip, int take)
        {
            return new Recipe[0];
        }

        public Recipe GetRecipe(long id)
        {
            return new Recipe { Id = id };
        }

        public Ingredient[] GetIngredients(int skip, int take)
        {
            return new Ingredient[0];
        }

        public Ingredient GetIngredient(long id)
        {
            return new Ingredient { Id = id, Name = "Recipe.Basil" };
        }
    }
}
