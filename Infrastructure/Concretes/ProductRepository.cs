using Application.Interfaces;
using Application.Product;
using Domain.Product;
using GenericRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Concretes;

public class ProductRepository(AppDbContext dbContext) : Repository<ProductEntity, DbContext>(dbContext), IProductRepository
{
}