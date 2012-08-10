namespace TecX.Unity.Tracking
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;

    public interface IRequest
    {
        IBuilderContext BuilderContext { get; }

        int Depth { get; }

        IRequest ParentRequest { get; }

        IDictionary<string, object> RequestContext { get; }

        Type Service { get; }

        ITarget Target { get; }
            
        IRequest CreateChild(Type service, IBuilderContext context);
    }
}