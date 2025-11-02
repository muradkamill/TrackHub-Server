using Application.Admin;
using Application.Customer;
using Application.Interfaces;
using Application.Person;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController(ISender sender) : ControllerBase
    {

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetById(CancellationToken cancellationToken)
        {
            var person = await sender.Send(new GetByIdQuery.GetByHeaderRequest(), cancellationToken);
            if (person.IsFailed)
            {
                return BadRequest(person.Errors.Select(x => x.Message));
            }

            return Ok(person.Value);
        }

        [Authorize]
        [HttpGet("get-balance")]
        public async Task<IActionResult> GetBalance(CancellationToken cancellationToken)
        {
            var balance = await sender.Send(new GetBalanceQuery.GetBalanceQueryRequest(), cancellationToken);
            if (balance.IsFailed)
            {
                return BadRequest(balance.Errors.Select(x => x.Message));
            }

            return Ok(balance.Value);
        }
        [Authorize]
        [HttpGet("get-owner-products")]
        public async Task<IActionResult> GetOwnerProducts(CancellationToken cancellationToken)
        {
            var products = await sender.Send(new GetOwnerProducts.GetOwnerProductsRequest(), cancellationToken);
            if (products.IsFailed)
            {
                return BadRequest(products.Errors.Select(x => x.Message));
            }

            return Ok(products.Value);
        }
        
        [Authorize]
        [HttpPut("change-profile-photo")]
        public async Task<IActionResult> UpdateById([FromForm]UpdateProfilePhoto.UpdateProfilePhotoRequest request,CancellationToken cancellationToken)
        {
            var person = await sender.Send(request, cancellationToken);
            if (person.IsFailed)
            {
                return BadRequest(person.Errors.Select(x => x.Message));
            }

            return Ok();
        }

        [Authorize]
        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromForm] UpdatePasswordCommand.UpdatePasswordCommandRequest request,CancellationToken cancellationToken)
        {
            var password =await sender.Send(request, cancellationToken);
            if (password.IsFailed)
            {
                return BadRequest(password.Errors.Select(x => x.Message));
            }
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteByHeader(CancellationToken cancellationToken)
        {
            var person = await sender.Send(new DeleteCommand.DeleteCommandRequest(), cancellationToken);
            if (person.IsFailed)
            {
                return BadRequest(person.Errors.Select(x => x.Message));
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost("create-courier-application")]
        public async Task<IActionResult> CreateCourierProfile([FromForm]CreateCourierApplication.CreateApplicationCourierRequest request,CancellationToken cancellationToken)
        {
            var courier= await sender.Send(request, cancellationToken);
            if (courier.IsFailed)
            {
                return BadRequest(courier.Errors.Select(x => x.Message));
            }

            return Created();
        }

        [Authorize]
        [HttpPost("suggestion-admin")]
        public async Task<IActionResult> SuggestionToAdmin(CreateSuggestionToAdmin.CreateSuggestionToAdminRequest request,CancellationToken cancellationToken)
        {
            var application = await sender.Send(request,cancellationToken);
            if (application.IsFailed)
            {
                return BadRequest(application.Errors.Select(x => x.Message));
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("create-comment")]
        public async Task<IActionResult> CreateComment(CreateComment.CreateCommentRequest request,CancellationToken cancellationToken)
        {
            var comment = await sender.Send(request,cancellationToken);
            if (comment.IsFailed)
            {
                return BadRequest(comment.Errors.Select(x => x.Message));
            }

            return Ok();
        }
        
    }
}
