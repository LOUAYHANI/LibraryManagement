using LibraryManagement.Api.Contracts.Members;
using LibraryManagement.Application.Loans;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [Route("api/members")]
    public class MembersController : ControllerBase
    {
        private readonly GetMemberLateFees _getMemberLateFees;

        public MembersController(GetMemberLateFees getMemberLateFees)
        {
            _getMemberLateFees = getMemberLateFees;
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