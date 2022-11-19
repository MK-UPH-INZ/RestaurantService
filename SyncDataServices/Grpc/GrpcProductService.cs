using AutoMapper;
using Grpc.Core;
using RestaurantService.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.SyncDataServices.Grpc
{
    public class GrpcProductService : GrpcProduct.GrpcProductBase
    {
        private readonly IProductRepo repository;
        private readonly IMapper mapper;

        public GrpcProductService(
            IProductRepo repository,
            IMapper mapper
        ) 
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public override Task<ProductResponse> GetProducts(
            GetProductListRequest request,
            ServerCallContext context
        )
        {
            var response = new ProductResponse();
            var productIds = request.ProductIds;
            var products = repository.GetProductsByIdList(productIds);

            foreach (var product in products)
            {
                response.Products.Add(mapper.Map<GrpcProductModel>(product));
            }

            return Task.FromResult(response);
        }
    }
}
