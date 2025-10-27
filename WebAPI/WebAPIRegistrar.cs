using Application.Admin;
using Application.Cart;
using Application.Category;
using Application.Interfaces;
using Application.Person;
using Application.Product;
using Application.SubCategory;
using Infrastructure.Concretes;
using Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace WebAPI;

public static class WebApiRegistrar
{
    public static IServiceCollection AddWebApi(this IServiceCollection service)
    {
        service.AddScoped<IPersonRepository, PersonRepository>();
        service.AddScoped<IUnitOfWork, UnitOfWork>();
        service.AddScoped<IProductRepository, ProductRepository>();
        service.AddScoped<ICategoryRepository, CategoryRepository>();
        service.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
        service.AddScoped<ICartRepository, CartRepository>();
        service.AddScoped<ICommentRepository, CommentRepository>();
        service.AddScoped<ISuggestionRepository, SuggestionRepository>();


        service.AddSingleton<IUserIdProvider, CustomUserIdProvider>();



        return service;
    }
}