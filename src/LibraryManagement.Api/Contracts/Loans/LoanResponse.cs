namespace LibraryManagement.Api.Contracts.Loans
{
    public class LoanResponse
    {
        public Guid Id { get; init; }
        public Guid MemberId { get; init; }
        public Guid BookCopyId { get; init; }
        public DateOnly BorrowedOn { get; init; }
        public DateOnly DueDate { get; init; }
    }
}
