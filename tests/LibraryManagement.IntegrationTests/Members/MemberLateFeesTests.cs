using System.Net;
using System.Net.Http.Json;
using LibraryManagement.Api.Contracts.Members;
using LibraryManagement.Domain.Members;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace LibraryManagement.IntegrationTests.Members
{
    public class MemberLateFeesTests
    {
        [Fact]
        public async Task Member_without_overdue_loans_has_no_late_fees()
        {
            await using var application = new LibraryApiFactory();
            var client = application.CreateClient();

            var createResponse = await client.PostAsJsonAsync("/api/members", new RegisterMemberRequest
            {
                Name = "Member 1",
                Plan = MembershipPlan.Standard
            });

            createResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

            var member = await createResponse.Content.ReadFromJsonAsync<MemberResponse>();
            member.ShouldNotBeNull();

            var response = await client.GetAsync($"/api/members/{member.Id}/late-fees");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<MemberLateFeesResponse>();

            result.ShouldNotBeNull();
            result.Amount.ShouldBe(0m);
            result.Currency.ShouldBe("EUR");
        }

        [Fact]
        public async Task Unknown_member_returns_not_found()
        {
            await using var application = new LibraryApiFactory();
            var client = application.CreateClient();

            var response = await client.GetAsync(
                "/api/members/99999999-9999-9999-9999-999999999999/late-fees");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}