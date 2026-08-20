
namespace LibraryManagement.Application.Loans.Exceptions
{
    public class NoAvailableCopyException : Exception
    {
        public NoAvailableCopyException(Guid bookId)
            : base($"No copy is available for book '{bookId}'.")
        {
        }
    }
}
