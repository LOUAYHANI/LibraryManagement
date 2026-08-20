
namespace LibraryManagement.Domain.Members
{
    public class Member
    {
        public Guid Id { get; }

        public string Name { get; }

        public MembershipPlan Plan { get; }

        public int MaxActiveLoans =>
            Plan == MembershipPlan.Student ? 5 : 3;

        public int LoanDurationDays =>
            Plan == MembershipPlan.Student ? 28 : 21;

        public Member(Guid id, string name, MembershipPlan plan)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            Id = id;
            Name = name;
            Plan = plan;
        }
    }
}
