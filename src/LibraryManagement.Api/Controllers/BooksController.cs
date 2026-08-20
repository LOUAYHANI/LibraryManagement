using LibraryManagement.Api.Contracts.Books;
using LibraryManagement.Application.Books;
using LibraryManagement.Domain.Books;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly RegisterBook _registerBook;
        private readonly GetBooks _getBooks;

        public BooksController(RegisterBook registerBook, GetBooks getBooks)
        {
            _registerBook = registerBook;
            _getBooks = getBooks;
        }

        [HttpPost]
        [ProducesResponseType<BookResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookResponse>> Register(RegisterBookRequest request, CancellationToken cancellationToken)
        {
            var book = await _registerBook.ExecuteAsync(
                request.Title,
                request.Author,
                request.CopyCount,
                cancellationToken);

            var response = ToResponse(book);

            return Created($"/api/books/{book.Id}", response);
        }

        [HttpGet]
        [ProducesResponseType<IReadOnlyCollection<BookResponse>>(StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyCollection<BookResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var books = await _getBooks.ExecuteAsync(cancellationToken);

            var response = books.Select(ToResponse).ToList();

            return Ok(response);
        }

        private static BookResponse ToResponse(Book book)
        {
            return new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                TotalCopies = book.Copies.Count,
                AvailableCopies = book.Copies.Count(x => x.IsAvailable)
            };
        }
    }
}