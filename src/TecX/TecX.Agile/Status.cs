using System.ComponentModel;

namespace TecX.Agile
{
    /// <summary>
    /// Enum for the status of story-cards and iterations
    /// </summary>
    [DefaultValue(Defined)]
    public enum Status
    {
        /// <summary>
        /// Task is identified
        /// </summary>
        [Description("Task is defined")]
        Defined = 0,
        /// <summary>
        /// Taks is part of an active iteration
        /// </summary>
        [Description("Task is part of an active iteration")]
        ToDo,
        /// <summary>
        /// Work on the task is in progress
        /// </summary>
        [Description("Work on the task is in progress")]
        InProgress,
        /// <summary>
        /// Development work on the task is completed
        /// </summary>
        [Description("Development work on the task is completed")]
        Completed,
        /// <summary>
        /// Result is approved by the customer
        /// </summary>
        [Description("Result is approved by the customer")]
        Accepted,
    }
}