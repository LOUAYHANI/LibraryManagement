using System.Net;
using System.Net.Http.Json;
using LibraryManagement.Api.Contracts.Loans;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace LibraryManagement.IntegrationTests.Loans
{
    public class BorrowLoanTests
    {
        [Fact]
        public async Task Borrowing_available_book_returns_created()
        {
            await using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var request = new BorrowBookRequest
            {
                MemberId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                BookId = Guid.Parse("22222222-2222-2222-2222-222222222222")
            };

            var response = await client.PostAsJsonAsync("/api/loans", request, CancellationToken.None);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var loan = await response.Content.ReadFromJsonAsync<LoanResponse>(
                CancellationToken.None);

            loan.ShouldNotBeNull();
            loan.MemberId.ShouldBe(request.MemberId);
            loan.DueDate.ShouldBe(loan.BorrowedOn.AddDays(21));
        }
    }
}
