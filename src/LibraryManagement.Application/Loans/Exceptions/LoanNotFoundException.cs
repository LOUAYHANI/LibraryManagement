
namespace LibraryManagement.Application.Loans.Exceptions
{
    public class LoanNotFoundException : Exception
    {
        public LoanNotFoundException(Guid loanId)
            : base($"Loan '{loanId}' was not found.")
        {
        }
    }
}