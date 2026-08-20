
namespace LibraryManagement.Domain.Loans
{
    public class Loan
    {
        public Guid Id { get; private set; }
        public Guid MemberId { get; private set; }
        public Guid BookCopyId { get; private set; }
        public DateOnly BorrowedOn { get; private set; }
        public DateOnly DueDate { get; private set; }
        public DateOnly? ReturnedOn { get; private set; }
        public decimal LateFeeAmount { get; private set; }

        public bool IsActive => ReturnedOn is null;
        private Loan()
        {
        }

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
