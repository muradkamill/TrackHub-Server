using Domain.Auth;
using Domain.Card;
using Domain.Product;

namespace Domain.Comment;

public class CommentEntity:AuditLogging
{
    public int Id { get; set; }
    public string PersonFin { get; set; } = default!;
    public int ProductId { get; set; }
    public string? Comment { get; set; } = default!;


    
    public PersonEntity Person { get; set; } = default!;
    public ProductEntity Product { get; set; } = default!;
}