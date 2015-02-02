namespace Hydra.Infrastructure
{
    public static class HttpContentTypes
    {
        public static readonly HttpContentType Json = new HttpContentType("application/json");

        public static readonly HttpContentType Multipart = new HttpContentType("multipart/form-data");
    }
}
