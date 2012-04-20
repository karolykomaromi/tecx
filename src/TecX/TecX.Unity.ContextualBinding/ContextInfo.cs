namespace TecX.Unity.ContextualBinding
{
    using System;

    public class ContextInfo : IDisposable
    {
        public ContextInfo(string key, object value)
        {
            this.Key = key;
            this.Value = value;
        }

        public string Key { get; set; }

        public object Value { get; set; }

        public void Dispose()
        {
            
        }
    }
}