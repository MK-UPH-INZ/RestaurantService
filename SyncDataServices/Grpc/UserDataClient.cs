using AutoMapper;
using Grpc.Net.Client;
using IdentityService;
using Microsoft.Extensions.Configuration;
using RestaurantService.Models;
using System;
using System.Collections.Generic;

namespace RestaurantService.SyncDataServices.Grpc
{
    public class UserDataClient : IUserDataClient
    {
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        public UserDataClient(
            IConfiguration configuration,
            IMapper mapper
        )
        {
            this.configuration = configuration;
            this.mapper = mapper;
        }
        public IEnumerable<User> ReturnAllUsers()
        {
            Console.WriteLine($"--> Calling GRPC Service {configuration["GrpcUser"]}");

            var channel = GrpcChannel.ForAddress(configuration["GrpcUser"]);
            var client = new GrpcUser.GrpcUserClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllUsers(request);
                return mapper.Map<IEnumerable<User>>(reply.User);
            } catch ( Exception e)
            {
                Console.WriteLine($"--> Could not call GRPC Server {e.Message}");
                return null;
            }
        }
    }
}
