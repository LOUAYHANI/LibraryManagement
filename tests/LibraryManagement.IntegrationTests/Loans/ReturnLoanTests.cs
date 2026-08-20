using System.Net;
using System.Net.Http.Json;
using LibraryManagement.Api.Contracts.Books;
using LibraryManagement.Api.Contracts.Loans;
using LibraryManagement.Api.Contracts.Members;
using LibraryManagement.Domain.Members;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace LibraryManagement.IntegrationTests.Loans
{
    public class ReturnLoanTests
    {
        [Fact]
        public async Task Returning_loan_returns_ok()
        {
            await using var application = new LibraryApiFactory();
            var client = application.CreateClient();

            var memberResponse = await client.PostAsJsonAsync("/api/members", new RegisterMemberRequest
            {
                Name = "Member 1",
                Plan = MembershipPlan.Standard
            });

            memberResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

            var member = await memberResponse.Content.ReadFromJsonAsync<MemberResponse>();
            member.ShouldNotBeNull();

            var bookResponse = await client.PostAsJsonAsync("/api/books", new RegisterBookRequest
            {
                Title = "Book 1",
                Author = "Author 1",
                CopyCount = 1
            });

            bookResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

            var book = await bookResponse.Content.ReadFromJsonAsync<BookResponse>();
            book.ShouldNotBeNull();

            var borrowResponse = await client.PostAsJsonAsync("/api/loans", new BorrowBookRequest
            {
                MemberId = member.Id,
                BookId = book.Id
            });

            borrowResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

            var loan = await borrowResponse.Content.ReadFromJsonAsync<LoanResponse>();
            loan.ShouldNotBeNull();

            var returnResponse = await client.PostAsync($"/api/loans/{loan.Id}/return", null);

            returnResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

            var result = await returnResponse.Content.ReadFromJsonAsync<ReturnBookResponse>();

            result.ShouldNotBeNull();
            result.OverdueDays.ShouldBe(0);
        }

        [Fact]
        public async Task Returning_same_loan_twice_returns_conflict()
        {
            await using var application = new LibraryApiFactory();
            var client = application.CreateClient();

            var memberResponse = await client.PostAsJsonAsync("/api/members", new RegisterMemberRequest
            {
                Name = "Member 1",
                Plan = MembershipPlan.Standard
            });

            var member = await memberResponse.Content.ReadFromJsonAsync<MemberResponse>();
            member.ShouldNotBeNull();

            var bookResponse = await client.PostAsJsonAsync("/api/books", new RegisterBookRequest
            {
                Title = "Book 1",
                Author = "Author 1",
                CopyCount = 1
            });

            var book = await bookResponse.Content.ReadFromJsonAsync<BookResponse>();
            book.ShouldNotBeNull();

            var borrowResponse = await client.PostAsJsonAsync("/api/loans", new BorrowBookRequest
            {
                MemberId = member.Id,
                BookId = book.Id
            });

            var loan = await borrowResponse.Content.ReadFromJsonAsync<LoanResponse>();
            loan.ShouldNotBeNull();

            var firstReturn = await client.PostAsync($"/api/loans/{loan.Id}/return", null);
            firstReturn.StatusCode.ShouldBe(HttpStatusCode.OK);

            var secondReturn = await client.PostAsync($"/api/loans/{loan.Id}/return", null);

            secondReturn.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        }
    }
}