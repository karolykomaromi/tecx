namespace Hydra.Features.Books
{
    using System.Collections.Generic;
    using Hydra.Queries;

    public class AllBooksQuery : IQuery<IEnumerable<BookViewModel>>
    {
    }
}