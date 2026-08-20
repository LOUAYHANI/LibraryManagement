using LibraryManagement.Application.Abstractions;
using LibraryManagement.Application.Books;
using LibraryManagement.Domain.Books;
using NSubstitute;
using Shouldly;

namespace LibraryManagement.Application.Tests.Books
{
    public class GetBooksTests
    {
        [Fact]
        public async Task Returns_registered_books()
        {
            var books = new List<Book>
            {
                new Book(Guid.NewGuid(), "Book 1", "Author 1", 2),
                new Book(Guid.NewGuid(), "Book 2", "Author 2", 1)
            };

            var bookRepository = Substitute.For<IBookRepository>();

            bookRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(books);

            var getBooks = new GetBooks(bookRepository);

            var result = await getBooks.ExecuteAsync(CancellationToken.None);

            result.Count.ShouldBe(2);
        }
    }
}