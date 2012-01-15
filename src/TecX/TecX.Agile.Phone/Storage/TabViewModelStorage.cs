namespace TecX.Agile.Phone.Storage
{
    using Caliburn.Micro;

    using TecX.Agile.Phone.ViewModels;

    public class TabViewModelStorage : StorageHandler<TabViewModel>
    {
        public override void Configure()
        {
            this.Id(x => x.DisplayName);

            this.Property(x => x.Text)
                .InPhoneState()
                .RestoreAfterActivation();
        }
    }
}
