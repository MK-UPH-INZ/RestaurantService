using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RestaurantService.Data;
using RestaurantService.DTO;
using RestaurantService.DTO.User.Events;
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
                case EventType.UserUpdated:
                    updateUser(message);
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
                case "User_Updated":
                    return EventType.UserUpdated;
                case "User_Deleted":
                    return EventType.UserDeleted;
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
                        Console.WriteLine("--> Got new user");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void updateUser(string userUpdatedMessage)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepo>();

                var userUpdatedDTO = JsonSerializer.Deserialize<UserUpdatedDTO>(userUpdatedMessage);

                try
                {
                    var existingUser = userRepository.GetUserByExternalId(userUpdatedDTO.Id);

                    if (existingUser == null)
                    {
                        var newUser = mapper.Map<User>(userUpdatedDTO);
                        userRepository.CreateUser(newUser);
                        userRepository.SaveChanges();
                        Console.WriteLine("--> Got new user");
                    }
                    else
                    {
                        mapper.Map<UserUpdatedDTO, User>(userUpdatedDTO, existingUser);
                        userRepository.SaveChanges();
                        Console.WriteLine("--> Updated user");
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
        UserUpdated,
        UserDeleted,
        Undetermined
    }
}
