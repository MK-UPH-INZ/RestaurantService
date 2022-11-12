using Microsoft.EntityFrameworkCore;
using RestaurantService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.Data
{
    public class RestaurantRepo : IRestaurantRepo
    {
        private readonly AppDbContext context;
        public RestaurantRepo(AppDbContext context)
        {
            this.context = context;
        }
        public void CreateRestaurant(Restaurant restaurant)
        {
            if(restaurant == null)
            {
                throw new ArgumentNullException(nameof(restaurant));
            }

            context.Restaurants.Add(restaurant);
        }

        public void RemoveRestaurant(Restaurant restaurant)
        {
            context.Restaurants.Remove(restaurant);
        }

        public IEnumerable<Restaurant> GetAllRestaurants()
        {
            return context.Restaurants.Include("Owner").ToList();
        }

        public Restaurant GetRestaurantById(int id)
        {
            return context.Restaurants.Include("Owner").FirstOrDefault(r => r.Id == id);
        }

        public bool SaveChanges()
        {
            return (context.SaveChanges() >= 0);
        }
    }
}
