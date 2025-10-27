using Application.Interfaces;
using Application.Person;
using Domain.Auth;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Concretes;

public class PersonRepository(AppDbContext dbContext):Repository<PersonEntity,DbContext>(dbContext),IPersonRepository
{
    
}