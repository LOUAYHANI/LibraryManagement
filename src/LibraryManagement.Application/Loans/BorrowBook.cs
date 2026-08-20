using LibraryManagement.Application.Abstractions;
using LibraryManagement.Application.Loans.Exceptions;
using LibraryManagement.Domain.Loans;

namespace LibraryManagement.Application.Loans
{
    public class BorrowBook
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly TimeProvider _timeProvider;
        private readonly IUnitOfWork _unitOfWork;
        public BorrowBook(IBookRepository bookRepository,
                        IMemberRepository memberRepository,
                        ILoanRepository loanRepository,
                        IUnitOfWork unitOfWork,
                        TimeProvider timeProvider)
        {
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
            _loanRepository = loanRepository;
            _unitOfWork = unitOfWork;
            _timeProvider = timeProvider;
        }

        public async Task<Loan> ExecuteAsync(Guid memberId, Guid bookId, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetByIdAsync(memberId, cancellationToken);
             
            if (member is null)
                throw new MemberNotFoundException(memberId);

            var book = await _bookRepository.GetByIdAsync(bookId, cancellationToken);

            if (book is null)
                throw new BookNotFoundException(bookId);

            var activeLoans = await _loanRepository.CountActiveLoansAsync(memberId, cancellationToken);

            if (activeLoans >= member.MaxActiveLoans)
                throw new LoanLimitReachedException(memberId);

            var copy = book.FindAvailableCopy();

            if (copy is null)
                throw new NoAvailableCopyException(bookId);

            copy.Lend();

            var today = DateOnly.FromDateTime(_timeProvider.GetLocalNow().DateTime);

            var loan = new Loan(
                Guid.NewGuid(),
                member.Id,
                copy.Id,
                today,
                member.LoanDurationDays);

            await _loanRepository.AddAsync(loan, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return loan;
        }
    }
}

