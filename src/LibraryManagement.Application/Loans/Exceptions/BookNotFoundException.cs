
namespace LibraryManagement.Application.Loans.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException(Guid bookId)
            : base($"Book '{bookId}' was not found.")
        {
        }
    }
}
