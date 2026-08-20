using LibraryManagement.Application.Abstractions;

namespace LibraryManagement.Infrastructure.Persistence
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _dbContext;

        public EfUnitOfWork(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}