namespace LibraryManagement.Api.Contracts.Books
{
    public class BookResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Author { get; init; } = string.Empty;
        public int TotalCopies { get; init; }
        public int AvailableCopies { get; init; }
    }
}