using Application.Customer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(ISender sender) : ControllerBase
    {
        [Authorize]
        [HttpPost("rate-product")]
        public async Task<IActionResult> RateProduct(RateProduct.RateProductRequest request,CancellationToken cancellation)
        {
            var cart = await sender.Send(request, cancellation);
            if (cart.IsFailed)
            {
                return BadRequest(cart.Errors.Select(x => x.Message));
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("rate-courier")]
        public async Task<IActionResult> RateCourier(RateCourier.RateCourierRequest request,CancellationToken cancellation)
        {
            var rate = await sender.Send(request, cancellation);
            if (rate.IsFailed)
            {
                return BadRequest(rate.Errors.Select(x => x.Message));
            }

            return Ok();
        }

        [Authorize]
        [HttpPut("cancel-cart")]
        public async Task<IActionResult> CancelCart(CancelCart.CancelCartRequest request,CancellationToken cancellation)
        {
            var cart = await sender.Send(request, cancellation);
            if (cart.IsFailed)
            {
                return BadRequest(cart.Errors.Select(x => x.Message));
            }

            return Ok();
        }
        
        
    }
}
