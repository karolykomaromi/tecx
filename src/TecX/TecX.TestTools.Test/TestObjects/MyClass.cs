using System.ComponentModel;

namespace TecX.TestTools.Test.TestObjects
{
    internal class MyClass : INotifyPropertyChanged
    {
        private string _myProperty;

        public string MyProperty
        {
            get { return _myProperty; }
            set
            {
                if (value == _myProperty)
                    return;

                _myProperty = value;
                PropertyChanged(this, new PropertyChangedEventArgs("MyProperty"));
                PropertyChanged(this, new PropertyChangedEventArgs("MyDerivedProperty"));
            }
        }

        public string MyDerivedProperty
        {
            get { return MyProperty + "Derived"; }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}