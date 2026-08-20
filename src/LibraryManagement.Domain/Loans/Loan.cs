
namespace LibraryManagement.Domain.Loans
{
    public class Loan
    {
        public Guid Id { get; }

        public Guid MemberId { get; }

        public Guid BookCopyId { get; }

        public DateOnly BorrowedOn { get; }

        public DateOnly DueDate { get; }

        public Loan(
            Guid id,
            Guid memberId,
            Guid bookCopyId,
            DateOnly borrowedOn,
            int loanDurationDays)
        {
            Id = id;
            MemberId = memberId;
            BookCopyId = bookCopyId;
            BorrowedOn = borrowedOn;
            DueDate = borrowedOn.AddDays(loanDurationDays);
        }
    }
}
