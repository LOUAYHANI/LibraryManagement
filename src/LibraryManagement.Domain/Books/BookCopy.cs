
namespace LibraryManagement.Domain.Books
{
    public class BookCopy
    {
        public Guid Id { get; private set; }
        public CopyState State { get; private set; }

        public bool IsAvailable => State == CopyState.OnShelf;

        private BookCopy()
        {
        }

        public BookCopy(Guid id)
        {
            Id = id;
            State = CopyState.OnShelf;
        }

        public void Lend()
        {
            State = CopyState.OnLoan;
        }
        public void ReturnToShelf()
        {
            State = CopyState.OnShelf;
        }
    }
}