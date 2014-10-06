namespace Recipe
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using System.Windows.Threading;
    using Recipe.Entities;

    public class RecipeServiceClient : ClientBase<IRecipeService>, IRecipeService
    {
        private readonly Dispatcher dispatcher;

        public RecipeServiceClient(Dispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null);

            this.dispatcher = dispatcher;
        }

        public IAsyncResult BeginGetIngredient(long id, AsyncCallback callback, object asyncState)
        {
            return this.Channel.BeginGetIngredient(id, result => this.dispatcher.BeginInvoke(() => callback(result)), asyncState);
        }

        public Ingredient EndGetIngredient(IAsyncResult result)
        {
            return this.Channel.EndGetIngredient(result);
        }

        public IAsyncResult BeginGetRecipe(long id, AsyncCallback callback, object asyncState)
        {
            return this.Channel.BeginGetRecipe(id, result => this.dispatcher.BeginInvoke(() => callback(result)), asyncState);
        }

        public Recipe EndGetRecipe(IAsyncResult result)
        {
            return this.Channel.EndGetRecipe(result);
        }

        public IAsyncResult BeginGetIngredients(int skip, int take, AsyncCallback callback, object asyncState)
        {
            return this.Channel.BeginGetIngredients(skip, take, result => this.dispatcher.BeginInvoke(() => callback(result)), asyncState);
        }

        public Ingredient[] EndGetIngredients(IAsyncResult result)
        {
            return this.Channel.EndGetIngredients(result);
        }

        public IAsyncResult BeginGetRecipes(int skip, int take, AsyncCallback callback, object asyncState)
        {
            return this.Channel.BeginGetRecipes(skip, take, result => this.dispatcher.BeginInvoke(() => callback(result)), asyncState);
        }

        public Recipe[] EndGetRecipes(IAsyncResult result)
        {
            return this.Channel.EndGetRecipes(result);
        }
    }
}
