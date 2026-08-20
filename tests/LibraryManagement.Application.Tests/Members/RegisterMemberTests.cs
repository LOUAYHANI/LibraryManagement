using LibraryManagement.Application.Abstractions;
using LibraryManagement.Application.Members;
using LibraryManagement.Domain.Members;
using NSubstitute;
using Shouldly;

namespace LibraryManagement.Application.Tests.Members
{
    public class RegisterMemberTests
    {
        [Fact]
        public async Task Registers_member()
        {
            var memberRepository = Substitute.For<IMemberRepository>();
            var registerMember = new RegisterMember(memberRepository);

            var member = await registerMember.ExecuteAsync(
                "Member 1",
                MembershipPlan.Standard,
                CancellationToken.None);

            member.Name.ShouldBe("Member 1");
            member.Plan.ShouldBe(MembershipPlan.Standard);

            await memberRepository.Received(1)
                .AddAsync(Arg.Is<Member>(x => x.Id == member.Id), Arg.Any<CancellationToken>());
        }
    }
}