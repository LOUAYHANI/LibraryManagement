
namespace LibraryManagement.Domain.Loans
{
    public class LoanAlreadyReturnedException : Exception
    {
        public LoanAlreadyReturnedException(Guid loanId)
            : base($"Loan '{loanId}' has already been returned.")
        {
        }
    }
}