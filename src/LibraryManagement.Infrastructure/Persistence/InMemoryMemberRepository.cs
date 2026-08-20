using LibraryManagement.Application.Abstractions;
using LibraryManagement.Domain.Members;

namespace LibraryManagement.Infrastructure.Persistence
{
    public class InMemoryMemberRepository : IMemberRepository
    {
        private readonly List<Member> _members = new();

        public Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_members.SingleOrDefault(x => x.Id == id));
        }

        public void Add(Member member)
        {
            _members.Add(member);
        }
    }
}
