
namespace LibraryManagement.Domain.Books
{
    public class Book
    {
        private readonly List<BookCopy> _copies = new();

        public Guid Id { get; }

        public string Title { get; }

        public string Author { get; }

        public IReadOnlyCollection<BookCopy> Copies => _copies;

        public Book(Guid id, string title, string author, int copyCount)
        {
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

    }
}
