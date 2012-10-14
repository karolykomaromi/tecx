namespace TecX.EnumClasses.Tests.TestObjects
{
    using System.Runtime.Serialization;

    [DataContract(Name = "SortOrder")]
    public enum SortOrderEnum
    {
        [EnumMember]
        Ascending,

        [EnumMember]
        Descending
    }
}