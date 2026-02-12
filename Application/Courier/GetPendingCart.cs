using Application.Cart;
using Application.Interfaces;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Courier;

public class GetPendingCart
{
    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double R = 6371;
        double dLat = ToRadians(lat2 - lat1);
        double dLon = ToRadians(lon2 - lon1);
        lat1 = ToRadians(lat1);
        lat2 = ToRadians(lat2);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double ToRadians(double deg) => deg * (Math.PI / 180);

    private static decimal GetRatePerKm(string vehicleType)
    {
        return vehicleType switch
        {
            "Bicycle" => 0.5m,
            "Motorcycle" => 0.7m,
            "Car" => 1.0m,
            "Van" => 1.5m,
            "Truck" => 2.0m,
            _ => 0.3m
        };
    }

    public class GetPendingPackageRequest : IRequest<Result<List<GetPendingPackageResponse>>>
    {
    }

    public class GetPendingPackageResponse()
    {
        public string PersonFin { get; set; } = default!;
        public string PersonName { get; set; } = default!;
        public string PersonSurname { get; set; } = default!;

        public string ProductName { get; set; } = default!;
        public int ProductId { get; set; }
        public double DistanceKm { get; set; }
        public decimal CourierFee { get; set; }
        public decimal Weight { get; set; }
        public int CartId { get; set; }
    }

    public class GetPendingPackageRequestHandler(
        IPersonRepository iPersonRepository,
        ICartRepository iCartRepository)
        : IRequestHandler<GetPendingPackageRequest, Result<List<GetPendingPackageResponse>>>
    {
        public async Task<Result<List<GetPendingPackageResponse>>> Handle(GetPendingPackageRequest request,
            CancellationToken cancellationToken)
        {
            var personFin = "1111111";

            var courier = await iPersonRepository.FirstOrDefaultAsync(x => x.Fin == personFin, cancellationToken);
            if (courier is null)
            {
                return Result.Fail("Courier is not found");
            }

            var carts = await iCartRepository
                .Where(x => x.OrderStatus == nameof(CreateOrderCommand.OrderStatus.CourierPending) && x.VehicleType == courier.VehicleType).Include(x => x.Product).Include(x => x.Person).AsNoTracking().ToListAsync(cancellationToken);
            var list = new List<GetPendingPackageResponse>();

            foreach (var cart in carts)
            {
                if (cart.Latitude == null || cart.Longitude == null)
                {
                    return Result.Fail("Location is false!");
                }

                var lat1 = cart.Product.Latitude;
                var long1 = cart.Product.Longitude;
                var lat2 = cart.Latitude.GetValueOrDefault();
                var long2 = cart.Longitude.GetValueOrDefault();
                var distanceKm = CalculateDistance(lat1, long1, lat2, long2);
                var ratePerKm = GetRatePerKm(cart.VehicleType!);
                var baseFee = 1.0m;
                var weightFee = cart.Product.Weight * 0.2m;
                var fee = baseFee + ((decimal)distanceKm * ratePerKm) + weightFee;
                list.Add(new GetPendingPackageResponse()
                {
                    Weight = cart.Product.Weight * cart.Quantity,
                    CourierFee = fee,
                    DistanceKm = distanceKm,
                    PersonFin = cart.PersonFin,
                    ProductId = cart.ProductId,
                    ProductName = cart.Product.Name,
                    CartId = cart.Id,
                    PersonName = cart.Person.Name,
                    PersonSurname = cart.Person.SurName,
                });
            }


            return Result.Ok(list);
        }
    }
}