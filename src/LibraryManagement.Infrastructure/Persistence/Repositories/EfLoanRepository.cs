using LibraryManagement.Application.Abstractions;
using LibraryManagement.Domain.Loans;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Repositories
{
    public class EfLoanRepository : ILoanRepository
    {
        private readonly LibraryDbContext _dbContext;

        public EfLoanRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Loan?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Loans.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<int> CountActiveLoansAsync(Guid memberId, CancellationToken cancellationToken)
        {
            return _dbContext.Loans.CountAsync(
                x => x.MemberId == memberId && x.ReturnedOn == null,
                cancellationToken);
        }

        public async Task<IReadOnlyCollection<Loan>> GetByMemberIdAsync(
            Guid memberId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Loans
                .Where(x => x.MemberId == memberId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Loan loan, CancellationToken cancellationToken)
        {
            await _dbContext.Loans.AddAsync(loan, cancellationToken);
        }
    }
}