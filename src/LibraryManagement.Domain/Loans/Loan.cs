
namespace LibraryManagement.Domain.Loans
{
    public class Loan
    {
        public Guid Id { get; }

        public Guid MemberId { get; }

        public Guid BookCopyId { get; }

        public DateOnly BorrowedOn { get; }

        public DateOnly DueDate { get; }
        public DateOnly? ReturnedOn { get; private set; }
        public bool IsActive => ReturnedOn is null;

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

        public int Return(DateOnly returnedOn)
        {
            if (!IsActive)
                throw new LoanAlreadyReturnedException(Id);

            ReturnedOn = returnedOn;

            return returnedOn > DueDate
                ? returnedOn.DayNumber - DueDate.DayNumber
                : 0;
        }
    }
}
