using LibraryManagement.Application.Abstractions;
using LibraryManagement.Application.Loans;
using LibraryManagement.Application.Loans.Exceptions;
using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Loans;
using LibraryManagement.Domain.Members;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using Shouldly;
using System;


namespace LibraryManagement.Application.Tests.Loans
{
    public class BorrowBookTests
    {
        [Fact]
        public async Task Borrows_available_copy_for_standard_member()
        {
            var member = new Member(Guid.NewGuid(), "Member 1", MembershipPlan.Standard);
            var book = new Book(Guid.NewGuid(), "Book 1", "Author 1", 1);

            var bookRepository = Substitute.For<IBookRepository>();
            var memberRepository = Substitute.For<IMemberRepository>();
            var loanRepository = Substitute.For<ILoanRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            memberRepository.GetByIdAsync(member.Id, Arg.Any<CancellationToken>()).Returns(member);
            bookRepository.GetByIdAsync(book.Id, Arg.Any<CancellationToken>()).Returns(book);
            loanRepository.CountActiveLoansAsync(member.Id, Arg.Any<CancellationToken>()).Returns(0);

            var timeProvider = new FakeTimeProvider(
                new DateTimeOffset(2026, 8, 20, 10, 0, 0, TimeSpan.Zero));

            var borrowBook = new BorrowBook(
                        bookRepository,
                        memberRepository,
                        loanRepository,
                        unitOfWork,
                        timeProvider);

            var loan = await borrowBook.ExecuteAsync(member.Id, book.Id, CancellationToken.None);

            var copy = book.Copies.Single();

            loan.MemberId.ShouldBe(member.Id);
            loan.BookCopyId.ShouldBe(copy.Id);
            loan.BorrowedOn.ShouldBe(new DateOnly(2026, 8, 20));
            loan.DueDate.ShouldBe(new DateOnly(2026, 9, 10));

            copy.State.ShouldBe(CopyState.OnLoan);

            await loanRepository.Received(1)
                .AddAsync(Arg.Is<Loan>(x => x.Id == loan.Id), Arg.Any<CancellationToken>());
            await unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Uses_student_loan_duration()
        {
            var member = new Member(Guid.NewGuid(), "Member 1", MembershipPlan.Student);
            var book = new Book(Guid.NewGuid(), "Book 1", "Author 1", 1);

            var bookRepository = Substitute.For<IBookRepository>();
            var memberRepository = Substitute.For<IMemberRepository>();
            var loanRepository = Substitute.For<ILoanRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            memberRepository.GetByIdAsync(member.Id, Arg.Any<CancellationToken>()).Returns(member);
            bookRepository.GetByIdAsync(book.Id, Arg.Any<CancellationToken>()).Returns(book);
            loanRepository.CountActiveLoansAsync(member.Id, Arg.Any<CancellationToken>()).Returns(0);

            var timeProvider = new FakeTimeProvider(
                new DateTimeOffset(2026, 8, 20, 10, 0, 0, TimeSpan.Zero));

            var borrowBook = new BorrowBook(
                        bookRepository,
                        memberRepository,
                        loanRepository,
                        unitOfWork,
                        TimeProvider.System);

            var loan = await borrowBook.ExecuteAsync(member.Id, book.Id, CancellationToken.None);

            loan.DueDate.ShouldBe(new DateOnly(2026, 9, 17)); 
            await unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Rejects_borrow_when_standard_member_reaches_limit()
        {
            var member = new Member(Guid.NewGuid(), "Member 1", MembershipPlan.Standard);
            var book = new Book(Guid.NewGuid(), "Book 1", "Author 1", 1);

            var bookRepository = Substitute.For<IBookRepository>();
            var memberRepository = Substitute.For<IMemberRepository>();
            var loanRepository = Substitute.For<ILoanRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            memberRepository.GetByIdAsync(member.Id, Arg.Any<CancellationToken>()).Returns(member);
            bookRepository.GetByIdAsync(book.Id, Arg.Any<CancellationToken>()).Returns(book);
            loanRepository.CountActiveLoansAsync(member.Id, Arg.Any<CancellationToken>()).Returns(3);

            var borrowBook = new BorrowBook(
                        bookRepository,
                        memberRepository,
                        loanRepository,
                        unitOfWork,
                        TimeProvider.System);

            await Should.ThrowAsync<LoanLimitReachedException>(() =>
                borrowBook.ExecuteAsync(member.Id, book.Id, CancellationToken.None));

            book.Copies.Single().State.ShouldBe(CopyState.OnShelf);

            await loanRepository.DidNotReceive()
                .AddAsync(Arg.Any<Loan>(), Arg.Any<CancellationToken>());

            await unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Rejects_borrow_when_student_member_reaches_limit()
        {
            var member = new Member(Guid.NewGuid(), "Member 1", MembershipPlan.Student);
            var book = new Book(Guid.NewGuid(), "Book 1", "Author 1", 1);

            var bookRepository = Substitute.For<IBookRepository>();
            var memberRepository = Substitute.For<IMemberRepository>();
            var loanRepository = Substitute.For<ILoanRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            memberRepository.GetByIdAsync(member.Id, Arg.Any<CancellationToken>()).Returns(member);
            bookRepository.GetByIdAsync(book.Id, Arg.Any<CancellationToken>()).Returns(book);
            loanRepository.CountActiveLoansAsync(member.Id, Arg.Any<CancellationToken>()).Returns(5);

            var borrowBook = new BorrowBook(
                        bookRepository,
                        memberRepository,
                        loanRepository,
                        unitOfWork,
                        TimeProvider.System);

            await Should.ThrowAsync<LoanLimitReachedException>(() =>
                borrowBook.ExecuteAsync(member.Id, book.Id, CancellationToken.None));

            await unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Rejects_borrow_when_no_copy_is_available()
        {
            var member = new Member(Guid.NewGuid(), "Member 1", MembershipPlan.Standard);
            var book = new Book(Guid.NewGuid(), "Book 1", "Author 1", 1);

            book.Copies.Single().Lend();

            var bookRepository = Substitute.For<IBookRepository>();
            var memberRepository = Substitute.For<IMemberRepository>();
            var loanRepository = Substitute.For<ILoanRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            memberRepository.GetByIdAsync(member.Id, Arg.Any<CancellationToken>()).Returns(member);
            bookRepository.GetByIdAsync(book.Id, Arg.Any<CancellationToken>()).Returns(book);
            loanRepository.CountActiveLoansAsync(member.Id, Arg.Any<CancellationToken>()).Returns(0);

            var borrowBook = new BorrowBook(
                        bookRepository,
                        memberRepository,
                        loanRepository,
                        unitOfWork,
                        TimeProvider.System);

            await Should.ThrowAsync<NoAvailableCopyException>(() =>
                borrowBook.ExecuteAsync(member.Id, book.Id, CancellationToken.None));

            await loanRepository.DidNotReceive()
                .AddAsync(Arg.Any<Loan>(), Arg.Any<CancellationToken>());

            await unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Rejects_borrow_when_member_does_not_exist()
        {
            var memberId = Guid.NewGuid();

            var bookRepository = Substitute.For<IBookRepository>();
            var memberRepository = Substitute.For<IMemberRepository>();
            var loanRepository = Substitute.For<ILoanRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            memberRepository.GetByIdAsync(memberId, Arg.Any<CancellationToken>())
                .Returns((Member?)null);

            var borrowBook = new BorrowBook(
                        bookRepository,
                        memberRepository,
                        loanRepository,
                        unitOfWork,
                        TimeProvider.System);

            await Should.ThrowAsync<MemberNotFoundException>(() =>
                borrowBook.ExecuteAsync(memberId, Guid.NewGuid(), CancellationToken.None));

            await unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Rejects_borrow_when_book_does_not_exist()
        {
            var member = new Member(Guid.NewGuid(), "Member 1", MembershipPlan.Standard);
            var bookId = Guid.NewGuid();

            var bookRepository = Substitute.For<IBookRepository>();
            var memberRepository = Substitute.For<IMemberRepository>();
            var loanRepository = Substitute.For<ILoanRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            memberRepository.GetByIdAsync(member.Id, Arg.Any<CancellationToken>()).Returns(member);
            bookRepository.GetByIdAsync(bookId, Arg.Any<CancellationToken>())
                .Returns((Book?)null);

            var borrowBook = new BorrowBook(
                        bookRepository,
                        memberRepository,
                        loanRepository,
                        unitOfWork,
                        TimeProvider.System);

            await Should.ThrowAsync<BookNotFoundException>(() =>
                borrowBook.ExecuteAsync(member.Id, bookId, CancellationToken.None));

            await unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
