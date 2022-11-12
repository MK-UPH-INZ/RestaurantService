using RestaurantService.Models;
using System.Collections.Generic;

namespace RestaurantService.SyncDataServices.Grpc
{
    public interface IUserDataClient
    {
        IEnumerable<User> ReturnAllUsers();
    }
}
