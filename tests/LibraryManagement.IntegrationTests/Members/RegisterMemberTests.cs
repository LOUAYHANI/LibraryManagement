using System.Net;
using System.Net.Http.Json;
using LibraryManagement.Api.Contracts.Members;
using LibraryManagement.Domain.Members;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace LibraryManagement.IntegrationTests.Members
{
    public class RegisterMemberTests
    {
        [Fact]
        public async Task Registers_standard_member()
        {
            await using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var request = new RegisterMemberRequest
            {
                Name = "Member 1",
                Plan = MembershipPlan.Standard
            };

            var response = await client.PostAsJsonAsync("/api/members", request);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var member = await response.Content.ReadFromJsonAsync<MemberResponse>();

            member.ShouldNotBeNull();
            member.Name.ShouldBe("Member 1");
            member.Plan.ShouldBe(MembershipPlan.Standard);
            member.MaxActiveLoans.ShouldBe(3);
            member.LoanDurationDays.ShouldBe(21);
        }
    }
}