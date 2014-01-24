using Infrastructure.ViewModels;

namespace Infrastructure.Client.Test.TestObjects
{
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