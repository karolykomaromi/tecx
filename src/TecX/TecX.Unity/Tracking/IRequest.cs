namespace TecX.Unity.Tracking
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;

    public interface IRequest
    {
        Type Service { get; }

        IRequest ParentRequest { get; }

        IBuilderContext Context { get; }

        ITarget Target { get; }

        int Depth { get; }

        ITarget GetNextTarget();

        IRequest CreateChild(Type service, IBuilderContext context, IEnumerable<ITarget> target);
    }
}