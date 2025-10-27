using Domain.Auth;
using Domain.Card;
using Domain.Category;
using Domain.Comment;
using Domain.SubCategory;

namespace Domain.Product;

public class ProductEntity : AuditLogging
{
    public int Id { get; set; }
    public string OwnerFin { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public decimal Weight { get; set; }

    public bool IsNew { get; set; }
    public decimal ProductRate { get; set; }
    public int RateQuantity { get; set; }


    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int StockQuantity { get; set; }
    public List<string> ImageUrls { get; set; } = default!;


    public int SubCategoryId { get; set; }
    public PersonEntity PersonEntity { get; set; } = default!;

    public SubCategoryEntity ProductToSubCategory { get; set; } = default!;
    public List<CartEntity> Cart { get; set; } = default!;

    public List<CommentEntity> Comment { get; set; } = default!;
}