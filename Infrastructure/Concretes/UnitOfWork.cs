using GenericRepository;
using IUnitOfWork = Application.Interfaces.IUnitOfWork;

namespace Infrastructure.Concretes;

public class UnitOfWork(AppDbContext dbContext):IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken=default!)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}