namespace Hydra.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Hydra.Features.Books;
    using Hydra.Queries;

    public class BooksController : Controller
    {
        private readonly IMediator mediator;

        public BooksController(IMediator mediator)
        {
            Contract.Requires(mediator != null);

            this.mediator = mediator;
        }

        public async Task<ActionResult> IndexAsync()
        {
            var t = this.mediator.QueryAsync(new AllBooksQuery());

            return this.View(await t);
        }

        public ActionResult Index()
        {
            IEnumerable<BookViewModel> books = this.mediator.Query(new AllBooksQuery());

            return this.View(books);
        }

        public ActionResult Enter()
        {
            BookViewModel book = new BookViewModel();

            return this.View(book);
        }

        [HttpPost]
        public ActionResult Enter(BookViewModel book)
        {
            Contract.Requires(book != null);

            return this.RedirectToAction("Index");
        }
    }
}