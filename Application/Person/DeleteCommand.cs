using Application.Interfaces;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Person;

public class DeleteCommand
{
    public class DeleteCommandRequest():IRequest<Result>
    {
    }
    
    public class DeleteCommandHandler(IHttpContextAccessor iHttpContextAccessor,IPersonRepository iPersonRepository,IUnitOfWork iUnitOfWork):IRequestHandler<DeleteCommandRequest,Result>
    {
        public async Task<Result> Handle(DeleteCommandRequest request, CancellationToken cancellationToken)
        {
            var fin = iHttpContextAccessor.HttpContext!.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(fin) || ! await iPersonRepository.AnyAsync(x=>x.Fin==fin,cancellationToken))
            {
                return Result.Fail("Person not found!");
            }
            await iPersonRepository.DeleteByIdAsync(fin);
            await iUnitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();


        }
    }
}