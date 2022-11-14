using RestaurantService.DTO.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.SyncDataServices.Http
{
    public interface IUserDataClient
    {
        Task SendRestaurantToUser(RestaurantReadDTO restaurant);
    }
}
