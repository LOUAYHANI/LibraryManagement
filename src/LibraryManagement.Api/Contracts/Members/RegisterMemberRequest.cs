using LibraryManagement.Domain.Members;

namespace LibraryManagement.Api.Contracts.Members
{
    public class RegisterMemberRequest
    {
        public string Name { get; set; } = string.Empty;
        public MembershipPlan Plan { get; set; }
    }
}