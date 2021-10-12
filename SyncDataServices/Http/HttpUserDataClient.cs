using Microsoft.Extensions.Configuration;
using RestaurantService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestaurantService.SyncDataServices.Http
{
    public class HttpUserDataClient : IUserDataClient
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public HttpUserDataClient(
            HttpClient httpClient,
            IConfiguration configuration
        ) {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }
        public async Task SendRestaurantToUser(RestaurantReadDTO restaurant)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(restaurant),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync($"{configuration["UserService"]}", httpContent);

            if( response.IsSuccessStatusCode )
            {
                Console.WriteLine("--> Sync POST to UserService was OK");
            } else
            {
                Console.WriteLine("--> Sync POST to UserService was NOT OK");
            }
        }
    }
}
