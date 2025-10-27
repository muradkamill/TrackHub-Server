using Domain.Auth;
using Domain.Comment;
using Domain.Product;

namespace Domain.Card;

public class CartEntity:AuditLogging
{
    public int Id { get; set; }
    public string PersonFin { get; set; } = default!;
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    
    public decimal Price { get; set; }
    
    public string OrderStatus { get; set; } = default!;
    
    public string? VehicleType { get; set; } = default!;
    public bool IsSelected { get; set; }


    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public string? CourierFin { get; set; } = default!;
    public decimal? CourierFee { get; set; }
    public DateTime? DeliveredDate { get; set; }



    

    
    public List<CommentEntity> Comments { get; set; }=default!;
    public PersonEntity Person { get; set; } = default!;
    public ProductEntity Product { get; set; } = default!;
}