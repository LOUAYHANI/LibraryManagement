using LibraryManagement.Domain.Books;

namespace LibraryManagement.Application.Abstractions
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Book?> GetByCopyIdAsync(Guid copyId, CancellationToken cancellationToken);
        Task AddAsync(Book book, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<Book>> GetAllAsync(CancellationToken cancellationToken);
    }
}
