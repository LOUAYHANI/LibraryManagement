using LibraryManagement.Application.Abstractions;
using LibraryManagement.Application.Loans;
using LibraryManagement.Application.Loans.Exceptions;
using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Loans;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using Shouldly;
using System;

namespace LibraryManagement.Application.Tests.Loans
{
    public class ReturnBookTests
    {
        [Fact]
        public async Task Returns_book_and_makes_copy_available()
        {
            var book = new Book(Guid.NewGuid(), "Book 1", "Author 1", 1);
            var copy = book.Copies.Single();
            copy.Lend();

            var loan = new Loan(
                Guid.NewGuid(),
                Guid.NewGuid(),
                copy.Id,
                new DateOnly(2026, 8, 1),
                21);

            var bookRepository = Substitute.For<IBookRepository>();
            var loanRepository = Substitute.For<ILoanRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            loanRepository.GetByIdAsync(loan.Id, Arg.Any<CancellationToken>()).Returns(loan);
            bookRepository.GetByCopyIdAsync(copy.Id, Arg.Any<CancellationToken>()).Returns(book);

            var timeProvider = new FakeTimeProvider(
                new DateTimeOffset(2026, 8, 20, 10, 0, 0, TimeSpan.Zero));

            var lateFeePolicy = new CappedDailyLateFeePolicy();

            var returnBook = new ReturnBook(
                        bookRepository,
                        loanRepository,
                        lateFeePolicy,
                        unitOfWork,
                        timeProvider);

            var overdueDays = await returnBook.ExecuteAsync(loan.Id, CancellationToken.None);

            overdueDays.ShouldBe(0);
            loan.ReturnedOn.ShouldBe(new DateOnly(2026, 8, 20));
            loan.IsActive.ShouldBeFalse();

            copy.IsAvailable.ShouldBeTrue();
            copy.State.ShouldBe(CopyState.OnShelf);
        }

        [Fact]
        public async Task Returns_overdue_days_when_book_is_late()
        {
            var book = new Book(Guid.NewGuid(), "Book 1", "Author 1", 1);
            var copy = book.Copies.Single();
            copy.Lend();

            var loan = new Loan(
                Guid.NewGuid(),
                Guid.NewGuid(),
                copy.Id,
                new DateOnly(2026, 7, 25),
                21);

            var bookRepository = Substitute.For<IBookRepository>();
            var loanRepository = Substitute.For<ILoanRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            loanRepository.GetByIdAsync(loan.Id, Arg.Any<CancellationToken>()).Returns(loan);
            bookRepository.GetByCopyIdAsync(copy.Id, Arg.Any<CancellationToken>()).Returns(book);

            var timeProvider = new FakeTimeProvider(
                new DateTimeOffset(2026, 8, 20, 10, 0, 0, TimeSpan.Zero));

            var lateFeePolicy = new CappedDailyLateFeePolicy();

            var returnBook = new ReturnBook(
                        bookRepository,
                        loanRepository,
                        lateFeePolicy,
                        unitOfWork,
                        timeProvider);

            var overdueDays = await returnBook.ExecuteAsync(loan.Id, CancellationToken.None);

            overdueDays.ShouldBe(5);
            copy.IsAvailable.ShouldBeTrue();
        }

        [Fact]
        public async Task Rejects_return_when_loan_does_not_exist()
        {
            var loanId = Guid.NewGuid();

            var bookRepository = Substitute.For<IBookRepository>();
            var loanRepository = Substitute.For<ILoanRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            loanRepository.GetByIdAsync(loanId, Arg.Any<CancellationToken>())
                .Returns((Loan?)null);
            var lateFeePolicy = new CappedDailyLateFeePolicy();

            var returnBook = new ReturnBook(
                        bookRepository,
                        loanRepository,
                        lateFeePolicy,
                        unitOfWork,
                        TimeProvider.System);

            await Should.ThrowAsync<LoanNotFoundException>(() =>
                returnBook.ExecuteAsync(loanId, CancellationToken.None));
        }
    }
}