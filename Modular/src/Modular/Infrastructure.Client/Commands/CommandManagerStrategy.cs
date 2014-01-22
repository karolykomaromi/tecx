namespace Infrastructure.Commands
{
    using Infrastructure.ViewModels;
    using Microsoft.Practices.ObjectBuilder2;

    public class CommandManagerStrategy : BuilderStrategy
    {
        public override void PostBuildUp(IBuilderContext context)
        {
            ViewModel vm = context.Existing as ViewModel;

            if (vm != null)
            {
                vm.CommandManager = context.NewBuildUp<ICommandManager>();
            }
        }
    }
}