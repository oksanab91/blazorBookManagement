using CatalogManagementAPIs.Models;
using System.Diagnostics;
using System.Linq.Expressions;

internal static class BooksData
{
    public static List<Book> Books { get; set; } = new List<Book>();

    public static void FillBooksCatalog()
    {
        Debug.WriteLine("fill data");
        var rng = new Random();
        Books = Enumerable.Range(1, 100).Select(index => new Book
        {
            Id = index,
            Author = "Book author " + index.ToString(),
            Title = "Book title " + index.ToString(),
            Genre = "Book genre " + index.ToString()
        })
        .ToList();
    }

    public static IEnumerable<Book> ReadBooksCatalog(int pageNumber, int pageSize, string? title = null, string? author = null, string? genre = null, string? sortBy = "Title", string? sortOrder = "asc")
    {
        var filteredBooks = Books?.AsQueryable();

        if (filteredBooks is null)
        {
            return [];
        }

        if (!string.IsNullOrEmpty(title))
        {
            filteredBooks = filteredBooks.Where(book => !string.IsNullOrEmpty(book.Title) && book.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(author))
        {
            filteredBooks = filteredBooks.Where(book => !string.IsNullOrEmpty(book.Author) && book.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(genre))
        {
            filteredBooks = filteredBooks.Where(book => !string.IsNullOrEmpty(book.Genre) && book.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            filteredBooks = sortOrder?.ToLower() == "desc" ? filteredBooks.OrderByDescending(sortBy) : filteredBooks.OrderBy(sortBy);
        }

        filteredBooks = filteredBooks.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        if (filteredBooks != null)
        {
            return [.. filteredBooks];
        }

        return [];
    }

    private static IQueryable<Book> OrderBy(this IQueryable<Book> source, string propertyName)
    {
        var param = Expression.Parameter(typeof(Book), "book");
        var property = Expression.Property(param, propertyName);
        var lambda = Expression.Lambda(property, param);
        var methodName = "OrderBy";
        var types = new Type[] { source.ElementType, lambda.Body.Type };
        var mce = Expression.Call(typeof(Queryable), methodName, types, source.Expression, lambda);
        return source.Provider.CreateQuery<Book>(mce);
    }

    private static IQueryable<Book> OrderByDescending(this IQueryable<Book> source, string propertyName)
    {
        var param = Expression.Parameter(typeof(Book), "book");
        var property = Expression.Property(param, propertyName);
        var lambda = Expression.Lambda(property, param);
        var methodName = "OrderByDescending";
        var types = new Type[] { source.ElementType, lambda.Body.Type };
        var mce = Expression.Call(typeof(Queryable), methodName, types, source.Expression, lambda);
        return source.Provider.CreateQuery<Book>(mce);
    }
}
