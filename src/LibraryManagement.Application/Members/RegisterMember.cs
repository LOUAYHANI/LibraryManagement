using LibraryManagement.Application.Abstractions;
using LibraryManagement.Domain.Members;

namespace LibraryManagement.Application.Members
{
    public class RegisterMember
    {
        private readonly IMemberRepository _memberRepository; 
        private readonly IUnitOfWork _unitOfWork;

        public RegisterMember(IMemberRepository memberRepository, IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Member> ExecuteAsync(string name, MembershipPlan plan, CancellationToken cancellationToken)
        {
            var member = new Member(Guid.NewGuid(), name, plan);

            await _memberRepository.AddAsync(member, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return member;
        }
    }
}