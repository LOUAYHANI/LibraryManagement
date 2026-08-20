using LibraryManagement.Domain.Members;
using Shouldly;

namespace LibraryManagement.Domain.Tests.Members
{
    public class MemberTests
    {
        [Fact]
        public void Standard_member_can_have_up_to_three_active_loans()
        {
            var member = new Member(
                Guid.NewGuid(),
                "Member 1",
                MembershipPlan.Standard);

            member.MaxActiveLoans.ShouldBe(3);
        }

        [Fact]
        public void Student_member_can_have_up_to_five_active_loans()
        {
            var member = new Member(
                Guid.NewGuid(),
                "Member 1",
                MembershipPlan.Student);

            member.MaxActiveLoans.ShouldBe(5);
        }

        [Fact]
        public void Standard_loan_period_is_twenty_one_days()
        {
            var member = new Member(
                Guid.NewGuid(),
                "Member 1",
                MembershipPlan.Standard);

            member.LoanDurationDays.ShouldBe(21);
        }

        [Fact]
        public void Student_loan_period_is_twenty_eight_days()
        {
            var member = new Member(
                Guid.NewGuid(),
                "Member 1",
                MembershipPlan.Student);

            member.LoanDurationDays.ShouldBe(28);
        }

        [Fact]
        public void Requires_name()
        {
            Should.Throw<ArgumentException>(() =>
                new Member(Guid.NewGuid(), "", MembershipPlan.Standard));
        }
    }
}
