using LibraryManagement.Application.Abstractions;
using LibraryManagement.Application.Books;
using LibraryManagement.Domain.Books;
using NSubstitute;
using Shouldly;

namespace LibraryManagement.Application.Tests.Books
{
    public class RegisterBookTests
    {
        [Fact]
        public async Task Registers_book_with_requested_copies()
        {
            var bookRepository = Substitute.For<IBookRepository>();
            var registerBook = new RegisterBook(bookRepository);

            var book = await registerBook.ExecuteAsync(
                "Book 1",
                "Author 1",
                3,
                CancellationToken.None);

            book.Title.ShouldBe("Book 1");
            book.Author.ShouldBe("Author 1");
            book.Copies.Count.ShouldBe(3);

            await bookRepository.Received(1)
                .AddAsync(Arg.Is<Book>(x => x.Id == book.Id), Arg.Any<CancellationToken>());
        }
    }
}