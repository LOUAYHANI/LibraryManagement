
namespace LibraryManagement.Domain.Books
{
    public class BookCopy
    {
        public Guid Id { get; }

        public CopyState State { get; private set; }

        public bool IsAvailable => State == CopyState.OnShelf;

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