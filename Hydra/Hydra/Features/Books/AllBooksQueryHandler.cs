namespace Hydra.Features.Books
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hydra.Queries;

    public class AllBooksQueryHandler : IQueryHandler<AllBooksQuery, IEnumerable<BookViewModel>>
    {
        public async Task<IEnumerable<BookViewModel>> Handle(AllBooksQuery query)
        {
            return await Task<IEnumerable<BookViewModel>>.Factory.StartNew(
                () => new[] { new BookViewModel { Title = "Programming WCF services", ASIN = "B0043D2DUK" } });
        }
    }
}