using Infrastructure.Interceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureRegistrar
{
   public static IServiceCollection AddInfrastructure(this IServiceCollection service,IConfiguration configuration)
   {
      service.AddDbContext<AppDbContext>(options =>
      {
         var connection = configuration["ConnectionStrings:DefaultConnection"];
         options.UseSqlServer(connection);
         options.AddInterceptors(new AuditLoggingInterceptor());
      });
      return service;
   }
}