using AutoMapper;
using Grpc.Core;
using RestaurantService.Data;
using System.Threading.Tasks;

namespace RestaurantService.SyncDataServices.Grpc
{
    public class GrpcRestaurantService : GrpcRestaurant.GrpcRestaurantBase
    {
        private readonly IRestaurantRepo repository;
        private readonly IMapper mapper;
        public GrpcRestaurantService(
            IRestaurantRepo repository,
            IMapper mapper
        )
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public override Task<RestaurantResponse> GetAllRestaurants(
            GetAllRestaurantsRequest request, 
            ServerCallContext context
        )
        {
            var response = new RestaurantResponse();
            var restaurats = repository.GetAllRestaurants();

            foreach (var restaurant in restaurats)
            {
                response.Restaurant.Add(mapper.Map<GrpcRestaurantModel>(restaurant));
            }

            return Task.FromResult(response);
        }
    }
}
