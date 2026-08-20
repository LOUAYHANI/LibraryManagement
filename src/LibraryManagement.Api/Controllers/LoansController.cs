using LibraryManagement.Api.Contracts.Loans;
using LibraryManagement.Application.Loans;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [Route("api/loans")]
    public class LoansController : ControllerBase
    {
        private readonly BorrowBook _borrowBook;
        private readonly ReturnBook _returnBook;

        public LoansController(BorrowBook borrowBook, ReturnBook returnBook)
        {
            _borrowBook = borrowBook;
            _returnBook = returnBook;
        }

        [HttpPost]
        [ProducesResponseType<LoanResponse>(StatusCodes.Status201Created)]
        public async Task<ActionResult<LoanResponse>> Borrow(BorrowBookRequest request, CancellationToken cancellationToken)
        {
            var loan = await _borrowBook.ExecuteAsync(request.MemberId, request.BookId, cancellationToken);

            var response = new LoanResponse
            {
                Id = loan.Id,
                MemberId = loan.MemberId,
                BookCopyId = loan.BookCopyId,
                BorrowedOn = loan.BorrowedOn,
                DueDate = loan.DueDate
            };

            return Created($"/api/loans/{loan.Id}", response);
        }

        [HttpPost("{id:guid}/return")]
        [ProducesResponseType<ReturnBookResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ReturnBookResponse>> Return(Guid id, CancellationToken cancellationToken)
        {
            var overdueDays = await _returnBook.ExecuteAsync(id, cancellationToken);

            return Ok(new ReturnBookResponse
            {
                OverdueDays = overdueDays
            });
        }
    }
}
