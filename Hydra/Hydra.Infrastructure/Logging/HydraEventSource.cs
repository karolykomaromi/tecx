namespace Hydra.Infrastructure.Logging
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Diagnostics.Tracing;

    [EventSource(Name = "Outlawtrail-Hydra")]
    public class HydraEventSource : EventSource
    {
        private static readonly Lazy<HydraEventSource> Instance = new Lazy<HydraEventSource>(() => new HydraEventSource());

        private HydraEventSource()
        {
        }

        public static HydraEventSource Log
        {
            get
            {
                Contract.Ensures(Contract.Result<HydraEventSource>() != null);

                return HydraEventSource.Instance.Value;
            }
        }

        [Event(
            1, 
            Message = "Starting up.", 
            Keywords = Keywords.Perf, 
            Level = EventLevel.Informational)]
        public void Startup()
        {
            this.WriteEvent(1);
        }

        [Event(
            2, 
            Message = "Shutting down.", 
            Keywords = Keywords.Perf, 
            Level = EventLevel.Informational)]
        public void Shutdown()
        {
            this.WriteEvent(2);
        }

        [Event(
            3, 
            Message = "Loading page {0}", 
            Opcode = EventOpcode.Start, 
            Task = Tasks.Page, 
            Keywords = Keywords.Page, 
            Level = EventLevel.Informational)]
        public void PageStart(string url)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(3, url);
            }
        }

        [Event(
            4, 
            Keywords = Keywords.Diagnostic, 
            Level = EventLevel.Warning)]
        public void Warning(string message)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(4, message);
            }
        }

        [Event(
            5, 
            Message = "An error occured.\r\n{0}", 
            Keywords = Keywords.Error, 
            Level = EventLevel.Error)]
        public void Error(string message)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(5, message);
            }
        }

        [Event(
            6, 
            Message = "A critical error occured.\r\n{0}", 
            Keywords = Keywords.Error, 
            Level = EventLevel.Critical)]
        public void Critical(string message)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(6, message);
            }
        }

        [Event(7, Message = "Container is missing a type mapping from '{0}' with name '{1}'.", Keywords = Keywords.Container, Task = Tasks.Resolution, Level = EventLevel.Informational)]
        public void MissingMapping(string fromType, string mappingName)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(7, fromType, mappingName);
            }
        }

        [Event(
            8, 
            Message = "Could not find resource type '{1}' in assembly '{0}'.", 
            Keywords = Keywords.Diagnostic | Keywords.Resources, 
            Level = EventLevel.Informational)]
        public void ResourceTypeNotFound(string assemblyName, string resourceTypeName)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(8, assemblyName, resourceTypeName);
            }
        }

        [Event(
            9, 
            Message = "Could not find public static property '{1}' on resource Type '{0}'.", 
            Keywords = Keywords.Diagnostic | Keywords.Resources, 
            Level = EventLevel.Informational)]
        public void ResourcePropertyNotFound(string assemblyQualifiedResourceTypeName, string propertyName)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(9, assemblyQualifiedResourceTypeName, propertyName);
            }
        }

        [Event(
            10, 
            Message = "Could not find property '{1}' on Type '{0}'.", 
            Keywords = Keywords.Diagnostic, 
            Level = EventLevel.Informational)]
        public void PropertyNotFound(string assemblyQualifiedTypeName, string propertyName)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(10, assemblyQualifiedTypeName, propertyName);
            }
        }

        [Event(
            11, 
            Message = "Culture changed to '{0}'", 
            Keywords = Keywords.Diagnostic, 
            Level = EventLevel.Informational)]
        public void CultureChanged(string culture)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(11, culture);
            }
        }

        [Event(
            12, 
            Message = "Job '{0}' scheduled for retry with trigger '{1}' at '{2}'.", 
            Task = Tasks.Job, 
            Keywords = Keywords.Retry | Keywords.Diagnostic, 
            Level = EventLevel.Informational)]
        public void JobScheduledForRetry(string jobKey, string triggerKey, string nextRunAt)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(12, jobKey, triggerKey, nextRunAt);
            }
        }

        [Event(
            13, 
            Message = "Job '{0}' finally failed.", 
            Task = Tasks.Job,
            Keywords = Keywords.Retry | Keywords.Diagnostic, 
            Level = EventLevel.Warning)]
        public void JobFinallyFailed(string jobKey)
        {
            if (this.IsEnabled())
            {
                this.WriteEvent(13, jobKey);
            }
        }

        public class Keywords
        {
            public const EventKeywords Page = (EventKeywords)1;

            public const EventKeywords DataBase = (EventKeywords)2;

            public const EventKeywords Diagnostic = (EventKeywords)4;

            public const EventKeywords Perf = (EventKeywords)8;

            public const EventKeywords Error = (EventKeywords)16;

            public const EventKeywords Container = (EventKeywords)32;

            public const EventKeywords Resources = (EventKeywords)64;

            public const EventKeywords Retry = (EventKeywords)128;

            public const EventKeywords Translation = (EventKeywords)256;
        }

        public class Tasks
        {
            public const EventTask Page = (EventTask)1;

            public const EventTask Query = (EventTask)2;

            public const EventTask Resolution = (EventTask)3;

            public const EventTask Job = (EventTask)4;
        }
    }
}