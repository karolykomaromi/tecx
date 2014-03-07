using Infrastructure.Reflection;

namespace Infrastructure.Client.Test.TestObjects
{
    public class HasNotListViewRelevantAttribute
    {
        [PropertyMeta(IsListViewRelevant = false)]
        public string ShouldBeIgnored { get; set; }
    }
}