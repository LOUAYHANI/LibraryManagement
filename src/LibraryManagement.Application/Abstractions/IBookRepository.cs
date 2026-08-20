using LibraryManagement.Domain.Books;

namespace LibraryManagement.Application.Abstractions
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
