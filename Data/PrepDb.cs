using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RestaurantService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if(!context.Restaurants.Any())
            {
                Console.WriteLine("Seeding Restaurant Data");

                context.Restaurants.AddRange(
                    new Restaurant() { Name="Restauracja 1" },
                    new Restaurant() { Name = "Restauracja 2" },
                    new Restaurant() { Name = "Restauracja 3" },
                    new Restaurant() { Name = "Restauracja 4" },
                    new Restaurant() { Name = "Restauracja 5" }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Restaurant Data is present");
            }
        }
    }
}
