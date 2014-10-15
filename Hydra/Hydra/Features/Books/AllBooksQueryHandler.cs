namespace Hydra.Features.Books
{
    using System.Collections.Generic;
    using Hydra.Queries;

    public class AllBooksQueryHandler : IQueryHandler<AllBooksQuery, IEnumerable<BookViewModel>>
    {
        public IEnumerable<BookViewModel> Handle(AllBooksQuery query)
        {
            return new[] { new BookViewModel { Title = "Programming WCF service", ASIN = "B0043D2DUK" } };
        }
    }
}