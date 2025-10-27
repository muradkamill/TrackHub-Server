namespace Domain;

public class AuditLogging
{
    public required DateTime CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeleteAt { get; set; }
}