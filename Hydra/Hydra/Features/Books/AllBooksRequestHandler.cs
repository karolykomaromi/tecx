namespace Hydra.Features.Books
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Hydra.Infrastructure.Mediator;

    public class AllBooksRequestHandler : IRequestHandler<AllBooksRequest, IEnumerable<BookViewModel>>
    {
        public async Task<IEnumerable<BookViewModel>> Handle(AllBooksRequest request)
        {
            return await Task<IEnumerable<BookViewModel>>.Factory.StartNew(
                () => new[] { new BookViewModel { Title = "Programming WCF services", ASIN = "B0043D2DUK" } });
        }
    }
}