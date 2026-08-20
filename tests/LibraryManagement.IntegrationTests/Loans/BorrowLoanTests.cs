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
    public class BorrowLoanTests
    {
        [Fact]
        public async Task Borrowing_available_book_returns_created()
        {
            await using var application = new LibraryApiFactory();
            var client = application.CreateClient();

            var memberRequest = new RegisterMemberRequest
            {
                Name = "Member 1",
                Plan = MembershipPlan.Standard
            };

            var memberResponse = await client.PostAsJsonAsync("/api/members", memberRequest);
            memberResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

            var member = await memberResponse.Content.ReadFromJsonAsync<MemberResponse>();
            member.ShouldNotBeNull();

            var bookRequest = new RegisterBookRequest
            {
                Title = "Book 1",
                Author = "Author 1",
                CopyCount = 2
            };

            var bookResponse = await client.PostAsJsonAsync("/api/books", bookRequest);
            bookResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

            var book = await bookResponse.Content.ReadFromJsonAsync<BookResponse>();
            book.ShouldNotBeNull();

            var borrowRequest = new BorrowBookRequest
            {
                MemberId = member.Id,
                BookId = book.Id
            };

            var response = await client.PostAsJsonAsync("/api/loans", borrowRequest);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var loan = await response.Content.ReadFromJsonAsync<LoanResponse>();

            loan.ShouldNotBeNull();
            loan.MemberId.ShouldBe(member.Id);
            loan.DueDate.ShouldBe(loan.BorrowedOn.AddDays(21));
        }
    }
}