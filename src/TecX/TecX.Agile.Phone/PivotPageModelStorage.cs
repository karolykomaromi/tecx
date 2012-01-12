namespace TecX.Agile.Phone
{
    using Caliburn.Micro;

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
