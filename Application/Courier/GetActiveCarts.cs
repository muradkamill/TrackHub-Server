using Application.Cart;
using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Courier;

public class GetActiveCarts
{
    public class GetActiveCartsRequest:IRequest<Result<IQueryable<GetActiveCartsRequestResponse>>>
    {

    }
    public class GetActiveCartsRequestResponse
    {
        public int Id { get; set; }
        public string PersonFin { get; set; } = default!;
        public string PersonName { get; set; } = default!;
        public string PersonSurName { get; set; } = default!;
        public string ProductName { get; set; } = default!;
        public int ProductId { get; set; }
        public decimal Weight { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string OrderStatus { get; set; } = default!;



        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public decimal CourierFee { get; set; }
        public DateTime DeliveredDate { get; set; }

    }


    public class GetActiveCartsRequestHandler(ICartRepository iCartRepository,IPersonRepository iPersonRepository,IProductRepository iProductRepository,IHttpContextAccessor httpContextAccessor):IRequestHandler<GetActiveCartsRequest,Result<IQueryable<GetActiveCartsRequestResponse>>>
    {
        public async Task<Result<IQueryable<GetActiveCartsRequestResponse>>> Handle(GetActiveCartsRequest request,
            CancellationToken cancellationToken)
        {
            var courierFin = httpContextAccessor.HttpContext!.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(courierFin))
                return Result.Fail("Unauthorized access!");

            var carts = iCartRepository.Where(x =>
                x.CourierFin == courierFin && x.OrderStatus == nameof(CreateOrderCommand.OrderStatus.CourierAccept)).ToList();

            var res = carts.Adapt<List<GetActiveCartsRequestResponse>>();
            foreach (var cart in res)
            {
                var product = await iProductRepository.FirstOrDefaultAsync(x => x.Id == cart.ProductId, cancellationToken);
                var owner =await iPersonRepository.FirstOrDefaultAsync(x=>x.Fin==product.OwnerFin, cancellationToken);
                cart.PersonSurName = owner.SurName;
                cart.PersonName = owner.Name;
                cart.ProductName = product.Name;
                cart.Weight = product.Weight * cart.Quantity;
            }
            return Result.Ok(res.AsQueryable());
        }
    }
}