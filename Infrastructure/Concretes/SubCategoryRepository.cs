using Application.Interfaces;
using Application.SubCategory;
using Domain.Category;
using Domain.Product;
using Domain.SubCategory;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Concretes;

public class SubCategoryRepository(AppDbContext dbContext):Repository<SubCategoryEntity,DbContext>(dbContext) , ISubCategoryRepository
{
    
}