namespace Hydra.Infrastructure.Mediator
{
    using System;
    using System.Collections.Generic;

    public static class RequestHandlers
    {
        public static readonly IReadOnlyCollection<Type> Pipeline = new[] { typeof(RequestHandlerPipeline<,>), typeof(ValidationRequestHandler<,>) };

        public static readonly IReadOnlyCollection<Type> None = Type.EmptyTypes;
    }
}