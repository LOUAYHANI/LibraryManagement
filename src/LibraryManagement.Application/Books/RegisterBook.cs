using LibraryManagement.Application.Abstractions;
using LibraryManagement.Domain.Books;

namespace LibraryManagement.Application.Books
{
    public class RegisterBook
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterBook(IBookRepository bookRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Book> ExecuteAsync(string title, string author, int copyCount, CancellationToken cancellationToken)
        {
            var book = new Book(Guid.NewGuid(), title, author, copyCount);

            await _bookRepository.AddAsync(book, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return book;
        }
    }
}