namespace Hydra.Features.Books
{
    using System.Collections.Generic;
    using Hydra.Infrastructure.Mediator;

    public class AllBooksRequest : IRequest<IEnumerable<BookViewModel>>
    {
    }
}