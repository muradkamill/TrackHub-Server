using Application.Cart;
using Application.Interfaces;
using Domain.Card;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Concretes;

public class CartRepository(AppDbContext dbContext):Repository<CartEntity,DbContext>(dbContext) ,ICartRepository
{
    
}