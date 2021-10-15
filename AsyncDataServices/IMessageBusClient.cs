using RestaurantService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewRestaurant(RestaurantPublishedDTO restaurantPublishedDTO);
    }
}
