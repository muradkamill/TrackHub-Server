using Application.SubCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController(ISender sender) : ControllerBase
    {
        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> GetAllSubCategories(CancellationToken cancellationToken)
        {
            var subCategories=await sender.Send(new GetAllSubCategoryQuery.GetAllSubCategoryRequest(), cancellationToken);
            if (subCategories.IsFailed)
            {
                return BadRequest(subCategories.Errors.Select(x => x.Message));
            }

            return Ok(subCategories.Value);
        }

        [HttpGet("{subCategoryName:required}")]
        public async Task<IActionResult> GetByNameSubCategories(string subCategoryName,CancellationToken cancellationToken)
        {
            var subCategory = await sender.Send(new GetByNameSubCategory.GetByNameSubCategoryRequest()
            {
                SubCategoryName = subCategoryName
            }, cancellationToken);
            if (subCategory.IsFailed)
            {
                return BadRequest(subCategory.Errors.Select(x => x.Message));
            }

            return Ok(subCategory.Value);
            

        }

        [HttpGet("{subCategoryId:int}/get-product-by-subcategory")]
        public async Task<IActionResult> GetByNameSubCategories(int subCategoryId,CancellationToken cancellationToken)
        {
            var products = await sender.Send(new GetProductsBySubCategory.GetProductsBySubCategoryRequest()
            {
                SubCategoryId = subCategoryId
            },cancellationToken);
            if (products.IsFailed)
            {
                return BadRequest(products.Errors.Select(x => x.Message));
            }

            return Ok(products.Value);
        }

        [AllowAnonymous]
        [HttpGet("{subCategoryId:int}/get-subcategory-name")]
        public async Task<IActionResult> GetSubCategoryName(int subCategoryId,CancellationToken cancellationToken)
        {
            var products = await sender.Send(new GetSubCategoryName.GetSubCategoryNameRequest
            {
                SubCategoryId = subCategoryId
            },cancellationToken);
            if (products.IsFailed)
            {
                return BadRequest(products.Errors.Select(x => x.Message));
            }

            return Ok(products.Value);
        }
        
        
        // [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateSubCategoryCommand.CreateSubCategoryRequest request,CancellationToken cancellationToken)
        {
            var newSubCategory = await sender.Send(request, cancellationToken);
            if (newSubCategory.IsFailed)
            {
                return BadRequest(newSubCategory.Errors.Select(x=>x.Message));
            }

            return Created();
        }
        
        
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateByNameSubCategory.UpdateByNameSubCategoryRequest request,CancellationToken cancellationToken)
        {
            var subCategory = await sender.Send(request, cancellationToken);
            if (subCategory.IsFailed)
            {
                return BadRequest(subCategory.Errors.Select(x => x.Message));
            }

            return Ok();
        }
        
        
        
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(DeleteSubCategoryByName.DeleteSubCategoryByNameRequest request,
            CancellationToken cancellationToken)
        {
            var newSubCategory = await sender.Send(request, cancellationToken);
            if (newSubCategory.IsFailed)
            {
                return BadRequest(newSubCategory.Errors.Select(x=>x.Message));
            }

            return Ok();
        }

    }
}
