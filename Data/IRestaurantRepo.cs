using RestaurantService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.Data
{
    public interface IRestaurantRepo
    {
        bool SaveChanges();

        IEnumerable<Restaurant> GetAllRestaurants();
        Restaurant GetRestaurantById(int id);
        void CreateRestaurant(Restaurant restaurant);
        void RemoveRestaurant(Restaurant restaurant);
    }
}
