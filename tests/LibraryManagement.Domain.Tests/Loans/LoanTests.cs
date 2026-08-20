using LibraryManagement.Domain.Loans;
using Shouldly;

namespace LibraryManagement.Domain.Tests.Loans
{
    public class LoanTests
    {
        [Fact]
        public void Calculates_due_date_from_loan_duration()
        {
            var borrowedOn = new DateOnly(2026, 8, 20);

            var loan = new Loan(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                borrowedOn,
                21);

            loan.DueDate.ShouldBe(new DateOnly(2026, 9, 10));
        }
    }
}
