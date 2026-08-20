
namespace LibraryManagement.Domain.Loans
{
    public interface ILateFeePolicy
    {
        decimal Calculate(int overdueDays);
    }
}