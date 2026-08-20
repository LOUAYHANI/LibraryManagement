using LibraryManagement.Domain.Members;

namespace LibraryManagement.Application.Abstractions
{
    public interface IMemberRepository
    {
        Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken); 
        Task AddAsync(Member member, CancellationToken cancellationToken);
    }
}
