
namespace LibraryManagement.Domain.Members
{
    public class Member
    {

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public MembershipPlan Plan { get; private set; }

        public int MaxActiveLoans =>
            Plan == MembershipPlan.Student ? 5 : 3;

        public int LoanDurationDays =>
            Plan == MembershipPlan.Student ? 28 : 21;

        private Member()
        {
            Name = string.Empty;
        }

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
