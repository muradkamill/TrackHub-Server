using Application.Cart;
using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Courier;

public class GetDeliveredCarts
{
    public class GetDeliveredCartsRequest:IRequest<Result<IQueryable<GetDeliveredCartsResponse>>>
    {

    }
    public class GetDeliveredCartsResponse
    {
        public int Id { get; set; }
        public string PersonFin { get; set; } = default!;
        public string PersonName { get; set; } = default!;
        public string PersonSurname { get; set; } = default!;

        public decimal Weight { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;

        public int Quantity { get; set; }

        public decimal Price { get; set; }
        public double DistanceKm { get; set; }


        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public decimal CourierFee { get; set; }
        public DateTime DeliveredDate { get; set; }

    }


    public class GetDeliveredCartsRequestHandler(IHttpContextAccessor httpContextAccessor,ICartRepository iCartRepository,IProductRepository iProductRepository,IPersonRepository iPersonRepository):IRequestHandler<GetDeliveredCartsRequest,Result<IQueryable<GetDeliveredCartsResponse>>>
    {
        public async Task<Result<IQueryable<GetDeliveredCartsResponse>>> Handle(GetDeliveredCartsRequest request, CancellationToken cancellationToken)
        {
            var courierFin = httpContextAccessor.HttpContext!.User.FindFirst("fin")?.Value;
            if (string.IsNullOrWhiteSpace(courierFin))
                return Result.Fail("Unauthorized access!");

            var carts = iCartRepository.Where(x =>
                x.CourierFin == courierFin && x.OrderStatus == nameof(CreateOrderCommand.OrderStatus.Delivered)).ToList();
            var res = carts.Adapt<List<GetDeliveredCartsResponse>>();

            foreach (var cart in res)
            {
                var product = await iProductRepository.FirstOrDefaultAsync(x => x.Id == cart.ProductId, cancellationToken);
                var owner =await iPersonRepository.FirstOrDefaultAsync(x=>x.Fin==product.OwnerFin, cancellationToken);
                cart.Weight = cart.Quantity * product.Weight;
                cart.ProductName = product.Name;
                cart.PersonName = owner.Name;
                cart.PersonSurname = owner.SurName;

                var lat1 = product.Latitude;
                var long1 = product.Longitude;
                var lat2 =  cart.Latitude;
                var long2 = cart.Longitude;
                cart.DistanceKm = GetPendingCart.CalculateDistance(lat1, long1, lat2, long2);
            }
            return Result.Ok(res.AsQueryable());
        }
    }


}