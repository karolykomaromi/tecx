namespace TecX.Agile.Phone.ViewModels
{
    using System;
    using System.Linq;

    using Caliburn.Micro;

    public class PivotPageViewModel : Conductor<IScreen>.Collection.OneActive
    {
        readonly Func<TabViewModel> createTab;

        public PivotPageViewModel(Func<TabViewModel> createTab)
        {
            this.createTab = createTab;
        }

        public int NumberOfTabs { get; set; }

        protected override void OnInitialize()
        {
            Enumerable.Range(1, this.NumberOfTabs).Apply(x =>
            {
                var tab = this.createTab();
                tab.DisplayName = "Item " + x;
                this.Items.Add(tab);
            });

            this.ActivateItem(this.Items[0]);
        }
    }
}
