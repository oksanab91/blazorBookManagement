using CatalogManagementAPIs.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CatalogManagementAPIs.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/books")]
    [ApiController]
    public class BooksController : Controller
    {
        [HttpGet]
        public IEnumerable<Book> Get([FromQuery] string? title = null, [FromQuery] string? author = null, [FromQuery] string? genre = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortBy = null, [FromQuery] string? sortOrder = "asc")
        {            
            return BooksData.ReadBooksCatalog(pageNumber, pageSize, title, author, genre, sortBy, sortOrder);
        }

        [HttpPost]
        [Route("book")]
        public IActionResult Post([FromBody] Book value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newId = BooksData.Books[BooksData.Books.Count - 1].Id + 1;
            var newBook = new Book { Id = newId, Title = value.Title, Author = value.Author, Genre = value.Genre };

            BooksData.Books.Add(newBook);

            return Ok(newBook);
        }

        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] Book value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var foundBook = BooksData.Books.Find(book => book.Id == id);

            if (foundBook is null)
            {
                return BadRequest();
            }
               
            foundBook.Title = value.Title;
            foundBook.Author = value.Author;
            foundBook.Genre = value.Genre;

            return Ok(foundBook);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(long id)
        {
            int ind = BooksData.Books.FindIndex(post => post.Id == id);

            if (ind < 0)
            {
                return NotFound();
            }

            BooksData.Books.RemoveAt(ind);

            return Ok(BooksData.Books);
        }
        
    }
}
