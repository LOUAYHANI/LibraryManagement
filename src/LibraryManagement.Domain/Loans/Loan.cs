
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
        public decimal LateFeeAmount { get; private set; }

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

        public int Return(DateOnly returnedOn, ILateFeePolicy lateFeePolicy)
        {
            if (!IsActive)
                throw new LoanAlreadyReturnedException(Id);

            var overdueDays = CalculateOverdueDays(returnedOn);

            ReturnedOn = returnedOn;
            LateFeeAmount = lateFeePolicy.Calculate(overdueDays);

            return overdueDays;
        }
        public int CalculateOverdueDays(DateOnly asOfDate)
        {
            return asOfDate > DueDate
                ? asOfDate.DayNumber - DueDate.DayNumber
                : 0;
        }
    }
}
