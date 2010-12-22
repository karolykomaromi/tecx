namespace System.ComponentModel
{
    public class PropertyChangingEventArgs : EventArgs
    {
        // Fields
        private readonly string _propertyName;

        // Methods
        public PropertyChangingEventArgs(string propertyName)
        {
            _propertyName = propertyName;
        }

        // Properties
        public virtual string PropertyName
        {
            get
            {
                return _propertyName;
            }
        }
    }


}
