namespace Hydra.Features.Books
{
    using System.Web;
    using Hydra.Infrastructure.I18n;

    public class BookViewModel
    {
        public string Title { get; set; }

        public string ASIN { get; set; }

        public PolyglotString Description { get; set; }

        public HttpPostedFileBase Cover { get; set; }
    }
}