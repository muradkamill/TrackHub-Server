using System.Reflection;
using System.Text;
using Application.Behavior;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Application;

public static class ApplicationRegistrar
{
    public static IServiceCollection AddApplication(this IServiceCollection service,IConfiguration configuration)
    {
        service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(conf =>
        {
            conf.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)) ,
                ValidateLifetime = true,
            };
        });
        service.AddAuthorization();
        service.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            conf.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        service.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        service.AddHttpContextAccessor();
        return service;
    }
}