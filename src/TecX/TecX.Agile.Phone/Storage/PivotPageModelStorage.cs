namespace TecX.Agile.Phone.Storage
{
    using Caliburn.Micro;

    using TecX.Agile.Phone.ViewModels;

    public class PivotPageModelStorage : StorageHandler<PivotPageViewModel>
    {
        public override void Configure()
        {
            this.ActiveItemIndex()
                .InPhoneState()
                .RestoreAfterViewLoad();
        }
    }
}
