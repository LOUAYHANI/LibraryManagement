using LibraryManagement.Application.Abstractions;
using LibraryManagement.Domain.Books;

namespace LibraryManagement.Application.Books
{
    public class RegisterBook
    {
        private readonly IBookRepository _bookRepository;

        public RegisterBook(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book> ExecuteAsync(string title, string author, int copyCount, CancellationToken cancellationToken)
        {
            var book = new Book(Guid.NewGuid(), title, author, copyCount);

            await _bookRepository.AddAsync(book, cancellationToken);

            return book;
        }
    }
}