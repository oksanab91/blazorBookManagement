using Microsoft.Extensions.Hosting;

namespace CatalogManagementAPIs.Models
{
    public class Book
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
    }
}
