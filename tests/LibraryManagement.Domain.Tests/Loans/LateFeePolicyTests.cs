
using LibraryManagement.Domain.Loans;
using Shouldly;

namespace LibraryManagement.Domain.Tests.Loans
{
    public class LateFeePolicyTests
    {
        private readonly CappedDailyLateFeePolicy _policy = new();

        [Fact]
        public void No_fee_without_overdue_days()
        {
            var fee = _policy.Calculate(0);

            fee.ShouldBe(0m);
        }

        [Fact]
        public void Calculates_daily_late_fee()
        {
            var fee = _policy.Calculate(5);

            fee.ShouldBe(1m);
        }

        [Fact]
        public void Fifty_overdue_days_reaches_fee_limit()
        {
            var fee = _policy.Calculate(50);

            fee.ShouldBe(10m);
        }

        [Fact]
        public void Fee_does_not_exceed_limit()
        {
            var fee = _policy.Calculate(51);

            fee.ShouldBe(10m);
        }
    }
}