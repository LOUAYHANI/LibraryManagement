using LibraryManagement.Application.Abstractions;
using LibraryManagement.Domain.Books;

namespace LibraryManagement.Infrastructure.Persistence
{
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly List<Book> _books = new();

        public Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_books.SingleOrDefault(x => x.Id == id));
        }
        public Task<Book?> GetByCopyIdAsync(Guid copyId, CancellationToken cancellationToken)
        {
            var book = _books.SingleOrDefault(x => x.FindCopy(copyId) is not null);

            return Task.FromResult(book);
        }
        public Task AddAsync(Book book, CancellationToken cancellationToken)
        {
            _books.Add(book);
            return Task.CompletedTask;
        }
        public Task<IReadOnlyCollection<Book>> GetAllAsync(CancellationToken cancellationToken)
        {
            IReadOnlyCollection<Book> books = _books.ToList();

            return Task.FromResult(books);
        }
    }
}
