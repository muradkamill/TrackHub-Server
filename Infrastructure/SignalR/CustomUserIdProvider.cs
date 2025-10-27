using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.SignalR;

public class CustomUserIdProvider:IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst("fin")?.Value ?? "";
    }
}