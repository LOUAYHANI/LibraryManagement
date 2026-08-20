using LibraryManagement.Application.Abstractions;
using LibraryManagement.Domain.Loans;



namespace LibraryManagement.Infrastructure.Persistence
{
    public class InMemoryLoanRepository : ILoanRepository
    {
        private readonly List<Loan> _loans = new();

        public Task<int> CountActiveLoansAsync(Guid memberId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_loans.Count(x => x.MemberId == memberId));
        }

        public Task AddAsync(Loan loan, CancellationToken cancellationToken)
        {
            _loans.Add(loan);
            return Task.CompletedTask;
        }
    }
}
