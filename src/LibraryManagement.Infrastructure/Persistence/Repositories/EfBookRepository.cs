using LibraryManagement.Application.Abstractions;
using LibraryManagement.Domain.Books;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Repositories
{
    public class EfBookRepository : IBookRepository
    {
        private readonly LibraryDbContext _dbContext;

        public EfBookRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Books
                .Include(x => x.Copies)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<Book?> GetByCopyIdAsync(Guid copyId, CancellationToken cancellationToken)
        {
            return _dbContext.Books
                .Include(x => x.Copies)
                .SingleOrDefaultAsync(x => x.Copies.Any(copy => copy.Id == copyId), cancellationToken);
        }

        public async Task<IReadOnlyCollection<Book>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Books
                .Include(x => x.Copies)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Book book, CancellationToken cancellationToken)
        {
            await _dbContext.Books.AddAsync(book, cancellationToken);
        }
    }
}