
namespace LibraryManagement.Application.Loans.Exceptions
{
    public class MemberNotFoundException : Exception
    {
        public MemberNotFoundException(Guid memberId)
            : base($"Member '{memberId}' was not found.")
        {
        }
    }
}
