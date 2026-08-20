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

        public LoansController(BorrowBook borrowBook)
        {
            _borrowBook = borrowBook;
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
    }
}
