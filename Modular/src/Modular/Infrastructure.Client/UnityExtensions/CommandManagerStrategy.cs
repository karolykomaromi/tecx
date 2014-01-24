namespace Infrastructure.UnityExtensions
{
    using Infrastructure.Commands;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.ObjectBuilder2;

    public class CommandManagerStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            ViewModel vm = context.Existing as ViewModel;

            if (vm != null)
            {
                vm.CommandManager = context.NewBuildUp<ICommandManager>();
            }
        }
    }
}