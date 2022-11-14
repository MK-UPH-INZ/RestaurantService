using RestaurantService.DTO.Product.Events;
using RestaurantService.DTO.Restaurant.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewRestaurant(RestaurantPublishedDTO restaurantPublishedDTO);
        void UpdateRestaurant(RestaurantUpdatedDTO restaurantUpdatedDTO);
        void DeleteRestaurant(RestaurantDeletedDTO restaurantDeletedDTO);
        void PublishNewProduct(ProductPublishedDTO productPublishedDTO);
        void UpdateProduct(ProductUpdatedDTO productUpdatedDTO);
        void DeleteProduct(ProductDeletedDTO productDeletedDTO);
    }
}
