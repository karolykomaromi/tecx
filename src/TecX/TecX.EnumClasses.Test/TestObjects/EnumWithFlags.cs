namespace TecX.EnumClasses.Test.TestObjects
{
    using System;
    using System.Runtime.Serialization;

    [Flags, DataContract]
    public enum EnumWithFlags
    {
        [EnumMember]
        None = 0,

        [EnumMember]
        One = 1,

        [EnumMember]
        Two = 2,

        [EnumMember]
        Four = 5
    }
}