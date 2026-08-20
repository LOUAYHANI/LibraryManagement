
namespace LibraryManagement.Domain.Loans
{
    public class CappedDailyLateFeePolicy : ILateFeePolicy
    {
        private const decimal DailyRate = 0.20m;
        private const decimal MaxFee = 10m;

        public decimal Calculate(int overdueDays)
        {
            if (overdueDays <= 0)
                return 0;

            var fee = overdueDays * DailyRate;

            return Math.Min(fee, MaxFee);
        }
    }
}