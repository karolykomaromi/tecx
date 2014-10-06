namespace Hydra.Controllers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Hydra.Models;
    using Raven.Client;

    public class BooksController : Controller
    {
        private readonly IDocumentSession documentSession;

        public BooksController(IDocumentSession documentSession)
        {
            Contract.Requires(documentSession != null);

            this.documentSession = documentSession;
        }

        public async Task<ActionResult> IndexAsync()
        {
            var books = this.documentSession.Query<Book>();

            Book book = await books.FirstOrDefaultAsync() ?? new Book { Title = "Programming WCF service", ASIN = "B0043D2DUK" };

            return this.View(book);
        }

        public ActionResult Index()
        {
            var books = this.documentSession.Query<Book>();

            Book book = books.FirstOrDefault() ?? new Book { Title = "Programming WCF service", ASIN = "B0043D2DUK" };

            return this.View(book);
        }
    }
}