using System.Net;
using System.Net.Http.Json;
using LibraryManagement.Api.Contracts.Loans;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace LibraryManagement.IntegrationTests.Loans
{
    public class ReturnLoanTests
    {
        [Fact]
        public async Task Returning_loan_makes_book_available_again()
        {
            await using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var borrowRequest = new BorrowBookRequest
            {
                MemberId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                BookId = Guid.Parse("22222222-2222-2222-2222-222222222222")
            };

            var borrowResponse = await client.PostAsJsonAsync("/api/loans", borrowRequest);
            borrowResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

            var loan = await borrowResponse.Content.ReadFromJsonAsync<LoanResponse>();
            loan.ShouldNotBeNull();

            var returnResponse = await client.PostAsync($"/api/loans/{loan.Id}/return", null);

            returnResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

            var result = await returnResponse.Content.ReadFromJsonAsync<ReturnBookResponse>();
            result.ShouldNotBeNull();
            result.OverdueDays.ShouldBe(0);
        }
    }
}