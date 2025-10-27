using Domain.Auth;
using Domain.Product;
using Domain.SubCategory;

namespace Domain.Category;

public class CategoryEntity :AuditLogging
{
    public int CategoryId { get; set; } = default!;
    public string CategoryName { get; set; } = default!;

    
    public List<SubCategoryEntity> CategoryToSubCategory { get; set; } = default!;
}