using LibraryManagement.Api.Contracts.Members;
using LibraryManagement.Application.Loans;
using LibraryManagement.Application.Members;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [Route("api/members")]
    public class MembersController : ControllerBase
    {
        private readonly GetMemberLateFees _getMemberLateFees;
        private readonly RegisterMember _registerMember;

        public MembersController(GetMemberLateFees getMemberLateFees, RegisterMember registerMember)
        {
            _getMemberLateFees = getMemberLateFees;
            _registerMember = registerMember;
        }

        [HttpPost]
        [ProducesResponseType<MemberResponse>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MemberResponse>> Register(
            RegisterMemberRequest request,
            CancellationToken cancellationToken)
        {
            var member = await _registerMember.ExecuteAsync(request.Name, request.Plan, cancellationToken);

            var response = new MemberResponse
            {
                Id = member.Id,
                Name = member.Name,
                Plan = member.Plan,
                MaxActiveLoans = member.MaxActiveLoans,
                LoanDurationDays = member.LoanDurationDays
            };

            return Created($"/api/members/{member.Id}", response);
        }

        [HttpGet("{id:guid}/late-fees")]
        [ProducesResponseType<MemberLateFeesResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MemberLateFeesResponse>> GetLateFees(Guid id, CancellationToken cancellationToken)
        {
            var amount = await _getMemberLateFees.ExecuteAsync(id, cancellationToken);

            return Ok(new MemberLateFeesResponse
            {
                Amount = amount
            });
        }
    }
}