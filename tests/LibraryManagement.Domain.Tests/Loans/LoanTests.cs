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

        [Fact]
        public void Return_on_due_date_has_no_overdue_days()
        {
            var policy = new CappedDailyLateFeePolicy();
            var loan = new Loan(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateOnly(2026, 8, 20),
                21);

            var overdueDays = loan.Return(new DateOnly(2026, 9, 10), policy);

            overdueDays.ShouldBe(0);
            loan.ReturnedOn.ShouldBe(new DateOnly(2026, 9, 10));
            loan.IsActive.ShouldBeFalse();
        }

        [Fact]
        public void Early_return_has_no_overdue_days()
        {
            var policy = new CappedDailyLateFeePolicy();
            var loan = new Loan(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateOnly(2026, 8, 20),
                21);

            var overdueDays = loan.Return(new DateOnly(2026, 9, 5), policy);

            overdueDays.ShouldBe(0);
        }

        [Fact]
        public void Calculates_overdue_days()
        {
            var policy = new CappedDailyLateFeePolicy();
            var loan = new Loan(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateOnly(2026, 8, 20),
                21);

            var overdueDays = loan.Return(new DateOnly(2026, 9, 15), policy);

            overdueDays.ShouldBe(5);
        }

        [Fact]
        public void Cannot_return_same_loan_twice()
        {
            var policy = new CappedDailyLateFeePolicy();
            var loan = new Loan(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateOnly(2026, 8, 20),
                21);

            loan.Return(new DateOnly(2026, 9, 10), policy);

            Should.Throw<LoanAlreadyReturnedException>(() =>
                loan.Return(new DateOnly(2026, 9, 11), policy));
        }
    }
}
