using LibraryManagement.Application.Abstractions;
using LibraryManagement.Application.Loans;
using LibraryManagement.Application.Loans.Exceptions;
using LibraryManagement.Domain.Loans;
using LibraryManagement.Domain.Members;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using Shouldly;

namespace LibraryManagement.Application.Tests.Loans
{
    public class GetMemberLateFeesTests
    {
        [Fact]
        public async Task Calculates_fee_for_active_overdue_loan()
        {
            var member = new Member(Guid.NewGuid(), "Member 1", MembershipPlan.Standard);

            var loan = new Loan(
                Guid.NewGuid(),
                member.Id,
                Guid.NewGuid(),
                new DateOnly(2026, 7, 25),
                21);

            var memberRepository = Substitute.For<IMemberRepository>();
            var loanRepository = Substitute.For<ILoanRepository>();

            memberRepository.GetByIdAsync(member.Id, Arg.Any<CancellationToken>()).Returns(member);
            loanRepository.GetByMemberIdAsync(member.Id, Arg.Any<CancellationToken>())
                .Returns(new List<Loan> { loan });

            var timeProvider = new FakeTimeProvider(
                new DateTimeOffset(2026, 8, 20, 10, 0, 0, TimeSpan.Zero));

            var lateFeePolicy = new CappedDailyLateFeePolicy();

            var getMemberLateFees = new GetMemberLateFees(
                memberRepository,
                loanRepository,
                lateFeePolicy,
                timeProvider);

            var total = await getMemberLateFees.ExecuteAsync(member.Id, CancellationToken.None);

            total.ShouldBe(1m);
        }

        [Fact]
        public async Task Uses_frozen_fee_for_returned_loan()
        {
            var member = new Member(Guid.NewGuid(), "Member 1", MembershipPlan.Standard);

            var loan = new Loan(
                Guid.NewGuid(),
                member.Id,
                Guid.NewGuid(),
                new DateOnly(2026, 7, 20),
                21);

            var lateFeePolicy = new CappedDailyLateFeePolicy();

            loan.Return(new DateOnly(2026, 8, 15), lateFeePolicy);

            var memberRepository = Substitute.For<IMemberRepository>();
            var loanRepository = Substitute.For<ILoanRepository>();

            memberRepository.GetByIdAsync(member.Id, Arg.Any<CancellationToken>()).Returns(member);
            loanRepository.GetByMemberIdAsync(member.Id, Arg.Any<CancellationToken>())
                .Returns(new List<Loan> { loan });

            var timeProvider = new FakeTimeProvider(
                new DateTimeOffset(2026, 8, 20, 10, 0, 0, TimeSpan.Zero));

            var getMemberLateFees = new GetMemberLateFees(
                memberRepository,
                loanRepository,
                lateFeePolicy,
                timeProvider);

            var total = await getMemberLateFees.ExecuteAsync(member.Id, CancellationToken.None);

            total.ShouldBe(1m);
            loan.LateFeeAmount.ShouldBe(1m);
        }

        [Fact]
        public async Task Sums_member_late_fees()
        {
            var member = new Member(Guid.NewGuid(), "Member 1", MembershipPlan.Standard);
            var lateFeePolicy = new CappedDailyLateFeePolicy();

            var activeLateLoan = new Loan(
                Guid.NewGuid(),
                member.Id,
                Guid.NewGuid(),
                new DateOnly(2026, 7, 25),
                21);

            var returnedLateLoan = new Loan(
                Guid.NewGuid(),
                member.Id,
                Guid.NewGuid(),
                new DateOnly(2026, 7, 20),
                21);

            returnedLateLoan.Return(new DateOnly(2026, 8, 15), lateFeePolicy);

            var activeOnTimeLoan = new Loan(
                Guid.NewGuid(),
                member.Id,
                Guid.NewGuid(),
                new DateOnly(2026, 8, 10),
                21);

            var loans = new List<Loan>
                        {
                            activeLateLoan,
                            returnedLateLoan,
                            activeOnTimeLoan
                        };

            var memberRepository = Substitute.For<IMemberRepository>();
            var loanRepository = Substitute.For<ILoanRepository>();

            memberRepository.GetByIdAsync(member.Id, Arg.Any<CancellationToken>()).Returns(member);
            loanRepository.GetByMemberIdAsync(member.Id, Arg.Any<CancellationToken>()).Returns(loans);

            var timeProvider = new FakeTimeProvider(
                new DateTimeOffset(2026, 8, 20, 10, 0, 0, TimeSpan.Zero));

            var getMemberLateFees = new GetMemberLateFees(
                memberRepository,
                loanRepository,
                lateFeePolicy,
                timeProvider);

            var total = await getMemberLateFees.ExecuteAsync(member.Id, CancellationToken.None);

            total.ShouldBe(2m);
        }

        [Fact]
        public async Task Rejects_when_member_does_not_exist()
        {
            var memberId = Guid.NewGuid();

            var memberRepository = Substitute.For<IMemberRepository>();
            var loanRepository = Substitute.For<ILoanRepository>();

            memberRepository.GetByIdAsync(memberId, Arg.Any<CancellationToken>())
                .Returns((Member?)null);

            var getMemberLateFees = new GetMemberLateFees(
                memberRepository,
                loanRepository,
                new CappedDailyLateFeePolicy(),
                TimeProvider.System);

            await Should.ThrowAsync<MemberNotFoundException>(() =>
                getMemberLateFees.ExecuteAsync(memberId, CancellationToken.None));
        }
    }
}