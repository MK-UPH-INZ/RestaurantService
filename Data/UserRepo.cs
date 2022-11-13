using RestaurantService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantService.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext context;
        public UserRepo(AppDbContext context)
        {
            this.context = context;
        }

        public void CreateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            context.Users.Add(user);
        }

        public bool ExternalUserExists(int userId)
        {
            return context.Users.Where(user => user.ExternalId == userId).Any();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return context.Users.ToList();
        }

        public User GetUserByExternalId(int externalId)
        {
            return context.Users.FirstOrDefault(user => user.ExternalId == externalId);
        }

        public User GetUserById(int id)
        {
            return context.Users.FirstOrDefault(user => user.Id == id);
        }

        public void RemoveUser(User user)
        {
            context.Users.Remove(user);
        }

        public bool SaveChanges()
        {
            return (context.SaveChanges() >= 0);
        }
    }
}
