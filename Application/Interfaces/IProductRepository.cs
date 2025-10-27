using Application.Product;
using Domain.Product;
using GenericRepository;

namespace Application.Interfaces;

public interface IProductRepository :IRepository<ProductEntity>
{
}