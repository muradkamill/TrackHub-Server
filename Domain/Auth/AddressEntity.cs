namespace Domain.Auth;


public class AddressEntity
{
    public string Country { get; set; } = default!;
    public string City { get; set; } = default!;
    public string? FullAddress { get; set; }


}