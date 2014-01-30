namespace Infrastructure.UnityExtensions
{
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.ObjectBuilder2;

    public class ListViewStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            DynamicListViewModel vm = context.Existing as DynamicListViewModel;

            if (vm != null)
            {
                ILanguageManager languageManager = context.NewBuildUp<ILanguageManager>();

                languageManager.LanguageChanged += vm.OnLanguageChanged;
            }
        }
    }
}