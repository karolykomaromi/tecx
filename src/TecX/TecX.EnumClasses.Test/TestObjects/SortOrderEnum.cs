namespace TecX.EnumClasses.Test.TestObjects
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