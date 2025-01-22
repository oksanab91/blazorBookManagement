using BlazorBookManagement.models;
using System.Net.Http.Json;


namespace BlazorBookManagement.Services
{
    public class BooksService(HttpClient client, IConfiguration Config, ILogger<BooksService> logger)
    {
        private readonly HttpClient client = client;
        private readonly ILogger<BooksService> logger = logger;
        private readonly string serviceEndpoint = $"{Config.GetValue<string>("BackendUrl")}/api/books";

        public async Task<IList<Book>> GetData(string? title = null, string? author = null, string? genre = null, int pageNumber = 1, int pageSize = 10, string? sortBy = null, string? sortOrder = "asc")
        {
            var query = "";

            if (!string.IsNullOrEmpty(title))
            {
                query = $"title={title}";
            }
            if (!string.IsNullOrEmpty(author))
            {
                query = query.Length > 0 ? $"{query}&author={author}" : $"author={author}";
            }
            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Length > 0 ? $"{query}&genre={genre}" : $"genre={genre}";

            }
            query = $"?{query}&pageNumber={pageNumber}&pageSize={pageSize}&sortBy={sortBy}&sortOrder={sortOrder}";

            try
            {
                return await client.GetFromJsonAsync<IList<Book>>(serviceEndpoint + query) ?? new List<Book>();
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Error fetching data from the server.");
                return [];
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred.");
                return [];
            }

        }

        public async Task AddItem(Book newBook)
        {
            await client.PostAsJsonAsync<Book>($"{serviceEndpoint}/book", newBook);
        }

        public async Task SaveItem(Book book)
        {
            await client.PutAsJsonAsync($"{serviceEndpoint}/{book.Id}", book);
        }

        public async Task DeleteItem(long id)
        {
            await client.DeleteAsync($"{serviceEndpoint}/{id}");
        }
    }
}
