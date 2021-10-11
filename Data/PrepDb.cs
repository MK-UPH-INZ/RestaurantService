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
                    new Restaurant() { 
                        Name = "Greenanic Smoothies",
                        Address = "ul. Jałowcowa 1034",
                        City = "Rybnik",
                        PostalCode = "44-207",
                        RegisterDate = DateTime.Parse("2018-02-19")
                    },
                    new Restaurant()
                    {
                        Name = "Bangalore Spices",
                        Address = "ul. Akwarelowa 33",
                        City = "Warszawa",
                        PostalCode = "04-517",
                        Voivodeship = "Mazowieckie",
                        RegisterDate = DateTime.Parse("2012-11-04")
                    },
                    new Restaurant()
                    {
                        Name = "Veganic Corner",
                        Address = "ul. Pisarka Mariana 56",
                        City = "Warszawa",
                        PostalCode = "03-984",
                        RegisterDate = DateTime.Parse("2014-06-28")
                    }
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
