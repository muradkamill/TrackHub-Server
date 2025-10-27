using Application.Admin;
using Application.Courier;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourierController(ISender sender) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOwnApplicationInfo(CancellationToken cancellationToken)
        {
            var application = await sender.Send(new GetOwnApplicationInfo.GetOwnApplicationInfoRequest(), cancellationToken);
            if (application.IsFailed)
            {
                return BadRequest(application.Errors.Select(x => x.Message));
            }

            return Ok(application.Value);
        }

        [Authorize(Roles = "Courier")]
        [HttpGet("get-pending-carts")]
        public async Task<IActionResult> GetPendingCart(CancellationToken cancellationToken)
        {
            var carts = await sender.Send(new GetPendingCart.GetPendingPackageRequest(),cancellationToken);
            if (carts.IsFailed)
            {
                return BadRequest(carts.Errors.Select(x => x.Message));
            }

            return Ok(carts.Value);
        }
        [Authorize(Roles = "Courier")]
        [HttpGet("get-active-carts")]
        public async Task<IActionResult> GetActiveCart(CancellationToken cancellationToken)
        {
            var carts = await sender.Send(new GetActiveCarts.GetActiveCartsRequest(),cancellationToken);
            if (carts.IsFailed)
            {
                return BadRequest(carts.Errors.Select(x => x.Message));
            }

            return Ok(carts.Value);
        }

        [Authorize(Roles = "Courier")]
        [HttpGet("get-delivered-carts")]
        public async Task<IActionResult> GetDeliveredCart(CancellationToken cancellationToken)
        {
            var carts = await sender.Send(new GetDeliveredCarts.GetDeliveredCartsRequest(),cancellationToken);
            if (carts.IsFailed)
            {
                return BadRequest(carts.Errors.Select(x => x.Message));
            }

            return Ok(carts.Value);
        }
        [Authorize(Roles = "Courier")]
        [HttpPut("confirm-pending-cart")]
        public async Task<IActionResult> ConfirmPendingCart(ApprovePendingCart.ApprovePendingPackageRequest request,CancellationToken cancellationToken)
        {
            var order = await sender.Send(request, cancellationToken);
            if (order.IsFailed)
            {
                return BadRequest(order.Errors.Select(x => x.Message));
            }

            return Ok();
        }
        [Authorize(Roles = "Courier")]
        [HttpPut("delivery-cart")]
        public async Task<IActionResult> DeliveredCart(DeliveredCart.DeliveredPackageRequest request,CancellationToken cancellationToken)
        {
            var order = await sender.Send(request, cancellationToken);
            if (order.IsFailed)
            {
                return BadRequest(order.Errors.Select(x => x.Message));
            }

            return Ok();
        }
        
        
    }
}
