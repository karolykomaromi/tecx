namespace Hydra.Controllers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading;
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
            ////var books = this.documentSession.Query<Book>();

            // TODO weberse 2014-10-07 lightweight client does not support firstordefaultasync
            ////Book book = await books.FirstOrDefaultAsync() ?? new Book { Title = "Programming WCF service", ASIN = "B0043D2DUK" };
            
            Task<Book> t = Task<Book>.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000);
                    return new Book { Title = "Programming WCF service", ASIN = "B0043D2DUK" };
                });

            return this.View(await t);
        }

        public ActionResult Index()
        {
            var books = this.documentSession.Query<Book>();

            Book book = books.FirstOrDefault() ?? new Book { Title = "Programming WCF service", ASIN = "B0043D2DUK" };

            return this.View(book);
        }
    }
}