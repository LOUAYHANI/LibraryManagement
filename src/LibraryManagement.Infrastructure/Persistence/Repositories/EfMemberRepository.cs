using LibraryManagement.Application.Abstractions;
using LibraryManagement.Domain.Members;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Repositories
{
    public class EfMemberRepository : IMemberRepository
    {
        private readonly LibraryDbContext _dbContext;

        public EfMemberRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Members.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task AddAsync(Member member, CancellationToken cancellationToken)
        {
            await _dbContext.Members.AddAsync(member, cancellationToken);
        }
    }
}