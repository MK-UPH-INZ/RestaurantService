using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RestaurantService.Data;
using RestaurantService.DTO;
using RestaurantService.Models;
using System;
using System.Text.Json;

namespace RestaurantService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IMapper mapper;

        public EventProcessor(
            IServiceScopeFactory scopeFactory,
            IMapper mapper
        )
        {
            this.scopeFactory = scopeFactory;
            this.mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.UserPublished:
                    addUser(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDTO>(notificationMessage);

            switch (eventType.Event)
            {
                case "User_Published":
                    return EventType.UserPublished;
                default:
                    return EventType.Undetermined;
            }
        }

        private void addUser(string userPublishedMessage)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepo>();

                var userPublishedDTO = JsonSerializer.Deserialize<UserPublishedDTO>(userPublishedMessage);

                try
                {
                    var user = mapper.Map<User>(userPublishedDTO);

                    if (!userRepository.ExternalUserExists(user.ExternalId))
                    {
                        userRepository.CreateUser(user);
                        userRepository.SaveChanges();
                        Console.WriteLine("Got new user");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }

    enum EventType
    {
        UserPublished,
        Undetermined
    }
}
