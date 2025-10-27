using Domain.Auth;
using Domain.Category;
using Domain.Product;

namespace Domain.SubCategory;

public class SubCategoryEntity:AuditLogging
{
    public int Id { get; set; }
    public string SubCategoryName { get; set; } = default!;

    
    

    public int CategoryId { get; set; }
    public CategoryEntity Category { get; set; } = default!;
    
    

    public List<ProductEntity> Products { get; set; } = default!;

}