using Application.Interfaces;
using Domain.Comment;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Concretes;

public class CommentRepository(AppDbContext dbContext):Repository<CommentEntity,DbContext>(dbContext),ICommentRepository
{
    
}