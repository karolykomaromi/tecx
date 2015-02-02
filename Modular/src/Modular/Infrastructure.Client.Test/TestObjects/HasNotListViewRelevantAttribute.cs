namespace Infrastructure.Client.Test.TestObjects
{
    using Infrastructure.Reflection;

    public class HasNotListViewRelevantAttribute
    {
        [PropertyMeta(IsListViewRelevant = false)]
        public string ShouldBeIgnored { get; set; }
    }
}