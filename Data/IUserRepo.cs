﻿using RestaurantService.Models;
using System.Collections.Generic;

namespace RestaurantService.Data
{
    public interface IUserRepo
    {
        bool SaveChanges();

        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        User GetUserByExternalId(int externalId);
        void CreateUser(User user);
        void RemoveUser(User user);
        bool ExternalUserExists(int userId);
    }
}
