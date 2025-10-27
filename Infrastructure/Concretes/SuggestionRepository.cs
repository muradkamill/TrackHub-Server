using Application.Admin;
using Application.Interfaces;
using Domain.Suggestion;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Concretes;

public class SuggestionRepository(AppDbContext dbContext):Repository<SuggestionEntity,DbContext>(dbContext),ISuggestionRepository
{
    
}