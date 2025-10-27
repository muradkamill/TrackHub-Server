using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Interceptions;

public class AuditLoggingInterceptor:SaveChangesInterceptor
{
    private void  Auditing(DbContext dbContext)
    {
        var entries=dbContext.ChangeTracker.Entries<AuditLogging>();
        foreach (var entityEntry in entries)
        {
            if (entityEntry.State==EntityState.Added)
            {
                entityEntry.Property(x => x.CreateAt).CurrentValue = DateTime.UtcNow;
            }
            if (entityEntry.State==EntityState.Modified)
            {
                entityEntry.Property(x => x.UpdateAt).CurrentValue = DateTime.UtcNow;
            }
        }
    }
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context != null) Auditing(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context != null) Auditing(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }


}