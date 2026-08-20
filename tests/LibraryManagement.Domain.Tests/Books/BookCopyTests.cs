using LibraryManagement.Domain.Books;
using Shouldly;

namespace LibraryManagement.Domain.Tests.Books
{
    public class BookCopyTests
    {
        [Fact]
        public void New_copy_is_available()
        {
            var copy = new BookCopy(Guid.NewGuid());

            copy.IsAvailable.ShouldBeTrue();
            copy.State.ShouldBe(CopyState.OnShelf);
        }

        [Fact]
        public void Lend_marks_copy_as_on_loan()
        {
            var copy = new BookCopy(Guid.NewGuid());

            copy.Lend();

            copy.IsAvailable.ShouldBeFalse();
            copy.State.ShouldBe(CopyState.OnLoan);
        }
    }
}
