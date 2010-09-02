namespace TecX.Unity.AutoRegistration
{
    /// <summary>
    /// Well known application part names: 
    /// UI composition parts, like Controller or View;
    /// design patterns, like Visitor or Proxy;
    /// general app parts, like Service or Validator
    /// </summary>
    public static class WellKnownAppParts
    {
        public static class UI
        {
            /// <summary>Controller</summary>
            public const string Controller = "Controller";

            /// <summary>View</summary>
            public const string View = "View";

            /// <summary>ViewModel</summary>
            public const string ViewModel = "ViewModel";

            /// <summary>Presenter</summary>
            public const string Presenter = "Presenter";
        }

        public static class General
        {
            /// <summary>Service</summary>
            public const string Service = "Service";

            /// <summary>Task</summary>
            public const string Task = "Task";

            /// <summary>Factory</summary>
            public const string Factory = "Factory";

            /// <summary>Validator</summary>
            public const string Validator = "Validator";

            /// <summary>Manager</summary>
            public const string Manager = "Manager";

            /// <summary>Extension</summary>
            public const string Extension = "Extension";

            /// <summary>Handler</summary>
            public const string Handler = "Handler";

            /// <summary>Event</summary>
            public const string Event = "Event";

            /// <summary>DomainEvent</summary>
            public const string DomainEvent = "DomainEvent";

            /// <summary>Provider</summary>
            public const string Provider = "Provider";

            /// <summary>Policy</summary>
            public const string Policy = "Policy";

            /// <summary>Config</summary>
            public const string Config = "Config";

            /// <summary>Driver</summary>
            public const string Driver = "Driver";

            /// <summary>Builder</summary>
            public const string Builder = "Builder";

            /// <summary>Request</summary>
            public const string Request = "Request";

            /// <summary>Reply</summary>
            public const string Reply = "Reply";

            /// <summary>Response</summary>
            public const string Response = "Response";

            /// <summary>Info</summary>
            public const string Info = "Info";

            /// <summary>Filter</summary>
            public const string Filter = "Filter";

            /// <summary>Element</summary>
            public const string Element = "Element";

            /// <summary>Description</summary>
            public const string Description = "Description";

            /// <summary>Message</summary>
            public const string Message = "Message";

            /// <summary>Logger</summary>
            public const string Logger = "Logger";
        }

        public static class DesignPatterns
        {
            /// <summary>Strategy</summary>
            public const string Strategy = "Strategy";

            /// <summary>Decorator</summary>
            public const string Decorator = "Decorator";

            /// <summary>Visitor</summary>
            public const string Visitor = "Visitor";

            /// <summary>Adapter</summary>
            public const string Adapter = "Adapter";

            /// <summary>Wrapper</summary>
            public const string Wrapper = "Wrapper";

            /// <summary>Singleton</summary>
            public const string Singleton = "Singleton";

            /// <summary>Bridge</summary>
            public const string Bridge = "Bridge";

            /// <summary>Facade</summary>
            public const string Facade = "Facade";

            /// <summary>Proxy</summary>
            public const string Proxy = "Proxy";

            /// <summary>Command</summary>
            public const string Command = "Command";

            /// <summary>Repository</summary>
            public const string Repository = "Repository";

            /// <summary>Specification</summary>
            public const string Specification = "Specification";
        }
    }
}
