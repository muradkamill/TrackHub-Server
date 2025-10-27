using Application.Cart;
using Application.Category.CRUD;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ISender sender) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetByNameCartForPerson(CancellationToken cancellationToken)
        {
            var cart = await sender.Send(new GetByNameCartData.GetAllCartDataRequest(), cancellationToken);
            if (cart.IsFailed)
            {
                return BadRequest(cart.Errors.Select(x => x.Message));
            }
        
            return Ok(cart.Value);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCartForPerson(CreateProductInCart.CreateProductInCardRequest request,CancellationToken cancellationToken)
        {
            var cart = await sender.Send(request, cancellationToken);
            if (cart.IsFailed)
            {
                return BadRequest(cart.Errors.Select(x => x.Message));
            }

            return Created();
        }

        [Authorize]
        [HttpPost("order-cart")]
        public async Task<IActionResult> CreateNewOrder(CreateOrderCommand.CreateOrderCommandRequest request,
            CancellationToken cancellationToken)
        {
            var order = await sender.Send(request, cancellationToken);
            if (order.IsFailed)
            {
                return BadRequest(order.Errors.Select(x => x.Message));
            }

            return Created();
        }
        [Authorize]
        [HttpPut("update-cart")]
        public async Task<IActionResult> UpdateCartForPerson(UpdateProductInCart.UpdateProductInCartRequest request,CancellationToken cancellationToken)
        {
            var cart = await sender.Send(request, cancellationToken);
            if (cart.IsFailed)
            {
                return BadRequest(cart.Errors.Select(x => x.Message));
            }

            return Ok();
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteCartForPerson( DeleteByProductName.DeleteByProductNameRequest request,CancellationToken cancellationToken)
        {
            var cart = await sender.Send(request, cancellationToken);
            if (cart.IsFailed)
            {
                return BadRequest(cart.Errors.Select(x => x.Message));
            }

            return Ok();
        }
        [Authorize]
        [HttpPut("change-cart-select")]
        public async Task<IActionResult> ChangeSelectCart(ChangeSelectCartCommand.ChangeSelectCartCommandRequest request,CancellationToken cancellationToken)
        {
            var cart = await sender.Send(request, cancellationToken);
            if (cart.IsFailed)
            {
                return BadRequest(cart.Errors.Select(x => x.Message));
            }

            return Ok();
        }
    }
}
