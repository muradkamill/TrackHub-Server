using Application.Admin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(ISender sender) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("pending-courier-application")]
        public async Task<IActionResult> GetPendingApplication(CancellationToken cancellationToken)
        {
            var pendingApplications =await sender.Send(new GetPendingApplications.GetPendingApplicationsRequest(),cancellationToken);
            if (pendingApplications.IsFailed)
            {
                return BadRequest(pendingApplications.Errors.Select(x => x.Message));
            }

            return Ok(pendingApplications.Value);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("get-suggestions")]
        public async Task<IActionResult> GetSuggestions(CancellationToken cancellationToken)
        {
            var pendingApplications =await sender.Send(new GetSuggestions.GetSuggestionsRequest(),cancellationToken);
            if (pendingApplications.IsFailed)
            {
                return BadRequest(pendingApplications.Errors.Select(x => x.Message));
            }

            return Ok(pendingApplications.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approve-application")]
        public async Task<IActionResult> ApproveApplication(ApprovePendingApplication.ApprovePendingApplicationRequest request,CancellationToken cancellationToken)
        {
            var application = await sender.Send(request, cancellationToken);
            if (application.IsFailed)
            {
                return BadRequest(application.Errors.Select(x => x.Message));
            }

            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("reject-application")]
        public async Task<IActionResult> RejectApplication(RejectPendingApplication.RejectPendingApplicationRequest request,CancellationToken cancellationToken)
        {
            var application = await sender.Send(request,cancellationToken);
            if (application.IsFailed)
            {
                return BadRequest(application.Errors.Select(x => x.Message));
            }

            return Ok();
        }
        
        
    }
}
