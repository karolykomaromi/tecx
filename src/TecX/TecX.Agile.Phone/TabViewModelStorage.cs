namespace TecX.Agile.Phone
{
    using Caliburn.Micro;

    public class TabViewModelStorage : StorageHandler<TabViewModel>
    {
        public override void Configure()
        {
            Id(x => x.DisplayName);

            Property(x => x.Text)
                .InPhoneState()
                .RestoreAfterActivation();
        }
    }
}
