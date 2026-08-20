
namespace LibraryManagement.Application.Loans.Exceptions
{
    public class LoanLimitReachedException : Exception
    {
        public LoanLimitReachedException(Guid memberId)
            : base($"Member '{memberId}' has reached the active loan limit.")
        {
        }
    }
}
