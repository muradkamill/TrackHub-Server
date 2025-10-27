using Application.Interfaces;
using Application.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> GetAllProduct(CancellationToken cancellationToken)
        {
            var allProduct =await sender.Send(new GetAllProductsQuery.GetAllQueryRequest(),cancellationToken);
            if (allProduct.IsFailed)
            {
                return BadRequest(allProduct.Errors.Select(x => x.Message));
            }
            return Ok(allProduct.Value);
        }

        [AllowAnonymous]
        [HttpGet("{productId:int}/get-comments")]
        public async Task<IActionResult> GetAllComments(int productId,CancellationToken cancellationToken)
        {
            var comments = await sender.Send(new GetProductComments.GetProductCommentsRequest()
            {
                ProductId = productId
            },cancellationToken);
            if (comments.IsFailed)
            {
                return BadRequest(comments.Errors.Select(x => x.Message));
            }
            return Ok(comments.Value);

        }


        [AllowAnonymous]
        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetProductById(int productId,CancellationToken cancellationToken)
        {
            var product =await sender.Send(new GetProductById.GetProductByIdRequest()
            {
                ProductId = productId
            },cancellationToken);
            if (product.IsFailed)
            {
                return BadRequest(product.Errors.Select(x => x.Message));
            }
            return Ok(product.Value);
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommand.CreateProductRequest request,CancellationToken cancellationToken)
        {
            var newProduct = await sender.Send(request, cancellationToken);
            if (newProduct.IsFailed)
            {
                return BadRequest(newProduct.Errors.Select(x => x.Message));
            }

            return Created();
        }


        [Authorize]
        [HttpPut("{productName:required}")]
        public async Task<IActionResult> UpdateByName(UpdateProductByName.UpdateProductParameters request,string productName,CancellationToken cancellationToken)
        {
            var product = await sender.Send(new UpdateProductByName.UpdateProductByNameRequest()
            {
                Parameters = request,
                ProductName = productName
            }, cancellationToken);
            if (product.IsFailed)
            {
                return BadRequest(product.Errors.Select(x => x.Message));
            }
            return Ok();
        }

        [Authorize]
        [HttpDelete("{productName:required}")]
        public async Task<IActionResult> DeleteByName(string productName,CancellationToken cancellationToken)
        {
            var product = await sender.Send(new DeleteProductByName.DeleteProductByNameRequest
            {
                ProductName =productName
            }, cancellationToken);
            if (product.IsFailed)
            {
                return BadRequest(product.Errors.Select(x => x.Message));
            }

            return Ok();
        }

    }
}
