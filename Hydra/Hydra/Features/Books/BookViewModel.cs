namespace Hydra.Features.Books
{
    using System.Web;

    public class BookViewModel
    {
        public string Title { get; set; }

        public string ASIN { get; set; }

        public HttpPostedFileBase Cover { get; set; }
    }
}