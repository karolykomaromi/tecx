namespace Infrastructure.Client.Test.TestObjects
{
    using Infrastructure.ViewModels;

    public class MultiLanguageViewModel : ViewModel
    {
        public string Foo
        {
            get { return this.Translate(() => this.Foo); }
        }

        public string LabelFoo
        {
            get { return this.Translate(() => this.LabelFoo); }
        }
    }
}