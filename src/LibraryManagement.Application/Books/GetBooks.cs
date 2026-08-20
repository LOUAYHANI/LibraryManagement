using LibraryManagement.Application.Abstractions;
using LibraryManagement.Domain.Books;

namespace LibraryManagement.Application.Books
{
    public class GetBooks
    {
        private readonly IBookRepository _bookRepository;

        public GetBooks(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<IReadOnlyCollection<Book>> ExecuteAsync(CancellationToken cancellationToken)
        {
            return _bookRepository.GetAllAsync(cancellationToken);
        }
    }
}