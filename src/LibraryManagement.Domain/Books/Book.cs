
namespace LibraryManagement.Domain.Books
{
    public class Book
    {
        private readonly List<BookCopy> _copies = new();

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public IReadOnlyCollection<BookCopy> Copies => _copies;

        private Book()
        {
            Title = string.Empty;
            Author = string.Empty;
        }

        public Book(Guid id, string title, string author, int copyCount)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.", nameof(title));

            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Author is required.", nameof(author));

            if (copyCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(copyCount), "Copy count must be greater than zero.");

            Id = id;
            Title = title;
            Author = author;

            for (var i = 0; i < copyCount; i++)
            {
                _copies.Add(new BookCopy(Guid.NewGuid()));
            }
        }

        public BookCopy? FindAvailableCopy()
        {
            return _copies.FirstOrDefault(copy => copy.IsAvailable);
        }
        public BookCopy? FindCopy(Guid copyId)
        {
            return _copies.SingleOrDefault(x => x.Id == copyId);
        }

    }
}
