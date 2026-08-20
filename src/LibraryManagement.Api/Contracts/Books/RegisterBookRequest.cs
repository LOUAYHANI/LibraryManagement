namespace LibraryManagement.Api.Contracts.Books
{
    public class RegisterBookRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int CopyCount { get; set; }
    }
}