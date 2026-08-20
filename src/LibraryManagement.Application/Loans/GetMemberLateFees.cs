
using LibraryManagement.Application.Abstractions;
using LibraryManagement.Application.Loans.Exceptions;
using LibraryManagement.Domain.Loans;

namespace LibraryManagement.Application.Loans
{
    public class GetMemberLateFees
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly ILateFeePolicy _lateFeePolicy;
        private readonly TimeProvider _timeProvider;

        public GetMemberLateFees(
            IMemberRepository memberRepository,
            ILoanRepository loanRepository,
            ILateFeePolicy lateFeePolicy,
            TimeProvider timeProvider)
        {
            _memberRepository = memberRepository;
            _loanRepository = loanRepository;
            _lateFeePolicy = lateFeePolicy;
            _timeProvider = timeProvider;
        }

        public async Task<decimal> ExecuteAsync(Guid memberId, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetByIdAsync(memberId, cancellationToken);

            if (member is null)
                throw new MemberNotFoundException(memberId);

            var loans = await _loanRepository.GetByMemberIdAsync(memberId, cancellationToken);
            var today = DateOnly.FromDateTime(_timeProvider.GetLocalNow().DateTime);

            decimal total = 0;

            foreach (var loan in loans)
            {
                if (loan.IsActive)
                {
                    var overdueDays = loan.CalculateOverdueDays(today);
                    total += _lateFeePolicy.Calculate(overdueDays);
                }
                else
                {
                    total += loan.LateFeeAmount;
                }
            }

            return total;
        }
    }
}