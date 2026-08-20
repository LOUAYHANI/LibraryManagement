namespace LibraryManagement.Api.Contracts.Loans
{
    public class BorrowBookRequest
    {
        public Guid MemberId { get; init; }
        public Guid BookId { get; init; }
    }
}
