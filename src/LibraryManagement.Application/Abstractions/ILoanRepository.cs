using LibraryManagement.Domain.Loans;

namespace LibraryManagement.Application.Abstractions
{
    public interface ILoanRepository
    {
        Task<int> CountActiveLoansAsync(Guid memberId, CancellationToken cancellationToken);

        Task AddAsync(Loan loan, CancellationToken cancellationToken);
        Task<Loan?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<Loan>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken);
    }
}
