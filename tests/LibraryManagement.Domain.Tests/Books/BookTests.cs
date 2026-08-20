using LibraryManagement.Domain.Books;
using Shouldly;

namespace LibraryManagement.Domain.Tests.Books
{
    public class BookTests
    {
        [Fact]
        public void Creates_requested_number_of_copies()
        {
            var book = new Book(
                Guid.NewGuid(),
                "Book 1",
                "Author 1",
                3);

            book.Copies.Count.ShouldBe(3);
        }

        [Fact]
        public void Returns_an_available_copy()
        {
            var book = new Book(
                Guid.NewGuid(),
                "Book 1",
                "Author 1",
                2);

            var copy = book.FindAvailableCopy();

            copy.ShouldNotBeNull();
            copy.IsAvailable.ShouldBeTrue();
        }

        [Fact]
        public void Returns_null_when_no_copy_is_available()
        {
            var book = new Book(
                Guid.NewGuid(),
                "Book 1",
                "Author 1",
                1);

            var copy = book.FindAvailableCopy();
            copy!.Lend();

            book.FindAvailableCopy().ShouldBeNull();
        }
    }
}
