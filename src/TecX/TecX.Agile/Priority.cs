using System.ComponentModel;

namespace TecX.Agile
{
    ///<summary>
    /// Priorities for work on planning artefacts
    ///</summary>
    [DefaultValue(NotRanked)]
    public enum Priority
    {
        /// <summary>
        /// Item has not been ranked yet
        /// </summary>
        [Description("Item has not been ranked yet")]
        NotRanked = 0,

        /// <summary>
        /// Item has a low priority
        /// </summary>
        [Description("Item has a low priority")]
        Low,

        /// <summary>
        /// Item has a medium priority
        /// </summary>
        [Description("Item has a medium priority")]
        Medium,

        /// <summary>
        /// Item has a high priority
        /// </summary>
        [Description("Item has a high priority")]
        High,

        /// <summary>
        /// Item must dealt with immediately
        /// </summary>
        [Description("Item must be dealt with immediately")]
        Immediate,
    }
}