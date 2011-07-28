namespace TecX.Agile.ViewModel.Test.TestObjects
{
    class TestViewModel : ViewModelBase
    {
        private string _foo;
        public string Foo
        {
            get { return _foo; }
            set
            {
                if (_foo == value)
                {
                    return;
                }

                _foo = value;
                OnPropertyChanged(() => Foo);
            }
        }
    }
}