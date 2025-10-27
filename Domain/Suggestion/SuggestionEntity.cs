namespace Domain.Suggestion;

public class SuggestionEntity:AuditLogging
{
    public int Id { get; set; }
    public string PersonFin { get; set; } = default!;
    public string Suggestion { get; set; } = default!;
    public string SuggestionStatus { get; set; } = default!;

}