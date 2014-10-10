namespace Hydra.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ScheduleNewJob
    {
        public Type SelectedJobType { get; set; }

        public IEnumerable<Type> JobTypes { get; set; }
    }
}