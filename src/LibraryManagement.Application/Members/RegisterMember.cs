using LibraryManagement.Application.Abstractions;
using LibraryManagement.Domain.Members;

namespace LibraryManagement.Application.Members
{
    public class RegisterMember
    {
        private readonly IMemberRepository _memberRepository;

        public RegisterMember(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<Member> ExecuteAsync(string name, MembershipPlan plan, CancellationToken cancellationToken)
        {
            var member = new Member(Guid.NewGuid(), name, plan);

            await _memberRepository.AddAsync(member, cancellationToken);

            return member;
        }
    }
}