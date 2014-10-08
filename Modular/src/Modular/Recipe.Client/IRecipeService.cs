namespace Recipe
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using Recipe.Entities;

    [ServiceContract]
    [ContractClass(typeof(RecipeServiceContract))]
    public interface IRecipeService
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetRecipes(int skip, int take, AsyncCallback callback, object asyncState);

        Recipe[] EndGetRecipes(IAsyncResult result);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetRecipe(long id, AsyncCallback callback, object asyncState);

        Recipe EndGetRecipe(IAsyncResult result);
        
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetIngredients(int skip, int take, AsyncCallback callback, object asyncState);

        Ingredient[] EndGetIngredients(IAsyncResult result);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetIngredient(long id, AsyncCallback callback, object asyncState);

        Ingredient EndGetIngredient(IAsyncResult result);
    }

    [ContractClassFor(typeof(IRecipeService))]
    internal abstract class RecipeServiceContract : IRecipeService
    {
        public IAsyncResult BeginGetIngredient(long id, AsyncCallback callback, object asyncState)
        {
            Contract.Requires(id > 0);

            return null;
        }

        public Ingredient EndGetIngredient(IAsyncResult result)
        {
            return null;
        }

        public IAsyncResult BeginGetRecipe(long id, AsyncCallback callback, object asyncState)
        {
            Contract.Requires(id > 0);

            return null;
        }

        public Recipe EndGetRecipe(IAsyncResult result)
        {
            return null;
        }

        public IAsyncResult BeginGetIngredients(int skip, int take, AsyncCallback callback, object asyncState)
        {
            Contract.Requires(skip >= 0);
            Contract.Requires(take > 0);

            return null;
        }

        public Ingredient[] EndGetIngredients(IAsyncResult result)
        {
            return null;
        }

        public IAsyncResult BeginGetRecipes(int skip, int take, AsyncCallback callback, object asyncState)
        {
            Contract.Requires(skip >= 0);
            Contract.Requires(take > 0);

            return null;
        }

        public Recipe[] EndGetRecipes(IAsyncResult result)
        {
            return null;
        }
    }
}