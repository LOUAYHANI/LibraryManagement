using LibraryManagement.Domain.Members;

namespace LibraryManagement.Api.Contracts.Members
{
    public class MemberResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public MembershipPlan Plan { get; init; }
        public int MaxActiveLoans { get; init; }
        public int LoanDurationDays { get; init; }
    }
}