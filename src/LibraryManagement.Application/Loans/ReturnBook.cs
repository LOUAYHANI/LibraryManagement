using LibraryManagement.Application.Abstractions;
using LibraryManagement.Application.Loans.Exceptions;
using LibraryManagement.Domain.Loans;

namespace LibraryManagement.Application.Loans
{
    public class ReturnBook
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly TimeProvider _timeProvider; 
        private readonly ILateFeePolicy _lateFeePolicy;

        public ReturnBook(
            IBookRepository bookRepository,
            ILoanRepository loanRepository,
            ILateFeePolicy lateFeePolicy,
            TimeProvider timeProvider)
        {
            _bookRepository = bookRepository;
            _loanRepository = loanRepository;
            _timeProvider = timeProvider;
            _lateFeePolicy = lateFeePolicy;
        }

        public async Task<int> ExecuteAsync(Guid loanId, CancellationToken cancellationToken)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId, cancellationToken);

            if (loan is null)
                throw new LoanNotFoundException(loanId);

            var book = await _bookRepository.GetByCopyIdAsync(loan.BookCopyId, cancellationToken);

            if (book is null)
                throw new InvalidOperationException($"Book copy '{loan.BookCopyId}' could not be found.");

            var copy = book.FindCopy(loan.BookCopyId);

            if (copy is null)
                throw new InvalidOperationException($"Book copy '{loan.BookCopyId}' could not be found.");

            var today = DateOnly.FromDateTime(_timeProvider.GetLocalNow().DateTime);

            var overdueDays = loan.Return(today, _lateFeePolicy);
            copy.ReturnToShelf();

            return overdueDays;
        }
    }
}