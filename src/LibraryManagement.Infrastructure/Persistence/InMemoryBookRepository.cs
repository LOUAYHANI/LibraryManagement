using LibraryManagement.Application.Abstractions;
using LibraryManagement.Domain.Books;

namespace LibraryManagement.Infrastructure.Persistence
{
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly List<Book> _books = new();

        public Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_books.FirstOrDefault(x => x.Id == id));
        }

        public void Add(Book book)
        {
            _books.Add(book);
        }
    }
}
