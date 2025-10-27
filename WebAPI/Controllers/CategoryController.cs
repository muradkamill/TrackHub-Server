using Application.Category;
using Application.Category.CRUD;
using Application.Person;
using Application.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ISender sender) : ControllerBase
    {
        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
        {
            var categories=await sender.Send(new GetAllCategoryQuery.GetAllCategoryRequest(), cancellationToken);
            if (categories.IsFailed)
            {
                return BadRequest(categories.Errors.Select(x => x.Message));
            }

            return Ok(categories.Value);
        }
        [AllowAnonymous]
        [HttpGet("{categoryName:required}")]
        public async Task<IActionResult> GetByNameCategories(string categoryName,CancellationToken cancellationToken)
        {
            var categories=await sender.Send(new GetByNameCategoryQuery.GetByIdCategoryRequest()
            {
                Name = categoryName
            }, cancellationToken);
            if (categories.IsFailed)
            {
                return BadRequest(categories.Errors.Select(x => x.Message));
            }

            return Ok(categories.Value);
        }
        [AllowAnonymous]
        [HttpGet("{categoryId:int}/get-products-by-subcategory")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId,CancellationToken cancellationToken)
        {
            var products=await sender.Send(new GetProductsByCategoryName.GetProductsByCategoryNameRequest()
            {
                CategoryId = categoryId
            }, cancellationToken);
            if (products.IsFailed)
            {
                return BadRequest(products.Errors.Select(x => x.Message));
            }

            return Ok(products.Value);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategory.CreateCategoryRequest request,CancellationToken cancellationToken)
        {
            var newCategory = await sender.Send(request, cancellationToken);
            if (newCategory.IsFailed)
            {
                return BadRequest(newCategory.Errors.Select(x=>x.Message));
            }

            return Created();
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("{categoryName:required}")]
        public async Task<IActionResult> UpdateCategory(string newCategoryName,string categoryName,CancellationToken cancellationToken)
        {
            var category = await sender.Send(new UpdateCategoryCommand.UpdateCategoryRequest()
            {
                CategoryName = categoryName,
                NewCategoryName = newCategoryName
            }, cancellationToken);
            if (category.IsFailed)
            {
                return BadRequest(category.Errors.Select(x => x.Message));
            }

            return Ok();

        }
        
        [Authorize(Roles = "Admin")]
        [HttpDelete("{categoryName:required}")]
        public async Task<IActionResult> DeleteCategoryByName(string categoryName,CancellationToken cancellationToken)
        {
            var category = await sender.Send(new DeleteByNameCommand.DeleteByNameCategoryRequest()
            {
                CategoryName = categoryName
            }, cancellationToken);
            if (category.IsFailed)
            {
                return BadRequest(category.Errors.Select(x => x.Message));
            }

            return Ok();

        }
        
        
    }
}
