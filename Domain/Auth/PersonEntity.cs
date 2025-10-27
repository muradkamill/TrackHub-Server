using Domain.Card;
using Domain.Product;

namespace Domain.Auth;

public class PersonEntity:AuditLogging
{
    public string Name { get; set; } = default!;
    public string SurName { get; set; } = default!;
    public string Mail { get; set; } = default!;

    public string Fin { get; set; } = default!;
    
    public string PasswordHash { get; set; } = default!;
    public string Role { get; set; } = default!;
    public decimal Balance { get; set; }
    public string? ImageUrl { get; set; } = default!;
    
    public string? RefreshToken { get; set; } 
    public DateTime? RefreshTokenExpiryDate { get; set; }
    
    public string? VehicleType { get; set; } = default!;
    public string? CvUrl { get; set; } = default!;
    
    public decimal CourierRate { get; set; }

    public int DeliveredPackageQuantity { get; set; }

    
    public string? ApplicationStatus { get; set; } = default!;



    
    public List<CartEntity> CartEntity { get; set; } = default!;

    public List<ProductEntity> ProductEntity { get; set; } = default!;
    // public List<OrderEntity> OrderEntity { get; set; } = default!;


}