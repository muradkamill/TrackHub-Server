using Application.Category;
using Application.Interfaces;
using Domain.Category;
using FluentResults;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Concretes;

public class CategoryRepository(AppDbContext dbContext):Repository<CategoryEntity,DbContext>(dbContext),ICategoryRepository
{
}