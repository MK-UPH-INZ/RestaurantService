using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RestaurantService.Models;
using RestaurantService.SyncDataServices.Grpc;
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
                var grpcClient = serviceScope.ServiceProvider.GetService<IUserDataClient>();
                var users = grpcClient.ReturnAllUsers();
                SeedUserData(
                    serviceScope.ServiceProvider.GetService<IUserRepo>(),
                    users
                );
                SeedRestaurantData(
                    serviceScope.ServiceProvider.GetService<AppDbContext>() 
                );
            }
        }

        private static void SeedUserData(
            IUserRepo userRepo,
            IEnumerable<User> users
        )
        {
            Console.WriteLine("Seeding User Data");

            if (users == null)
            {
                return;
            }

            foreach (var user in users)
            {
                if (!userRepo.ExternalUserExists(user.ExternalId))
                {
                    userRepo.CreateUser(user);
                }
            }

            userRepo.SaveChanges();
        }

        private static void SeedRestaurantData(
            AppDbContext context
        )
        {
            if(!context.Restaurants.Any())
            {
                Console.WriteLine("Seeding Restaurant Data");

                var user = context.Users.FirstOrDefault(user => user.ExternalId == 1);

                if (user == null)
                    return;

                context.Restaurants.AddRange(
                    new Restaurant() {
                        OwnerId = 1,
                        Name = "Greenanic Smoothies",
                        Address = "ul. Jałowcowa 1034",
                        City = "Rybnik",
                        PostalCode = "44-207",
                        RegisterDate = DateTime.Parse("2018-02-19")
                    },
                    new Restaurant()
                    {
                        OwnerId = 1,
                        Name = "Bangalore Spices",
                        Address = "ul. Akwarelowa 33",
                        City = "Warszawa",
                        PostalCode = "04-517",
                        Voivodeship = "Mazowieckie",
                        RegisterDate = DateTime.Parse("2012-11-04")
                    },
                    new Restaurant()
                    {
                        OwnerId = 1,
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
