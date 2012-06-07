namespace TecX.Unity.ContextualBinding
{
    public class ContextInfo
    {
        public ContextInfo(string key, object value)
        {
            this.Key = key;
            this.Value = value;
        }

        public string Key { get; set; }

        public object Value { get; set; }
    }
}