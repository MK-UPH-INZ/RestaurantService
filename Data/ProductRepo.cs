using Microsoft.EntityFrameworkCore;
using RestaurantService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantService.Data
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext context;
        public ProductRepo(AppDbContext context)
        {
            this.context = context;
        }

        public void CreateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            context.Products.Add(product);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return context.Products.Include("Restaurant.Owner").ToList();
        }

        public Product GetProductById(int id)
        {
            return context.Products.Include("Restaurant.Owner").FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetProductsByIdList(IEnumerable<int> restaurantIds)
        {
            return context.Products
                .Include("Restaurant.Owner")
                .Where(p => restaurantIds.Contains(p.Id))
                .ToList();
        }

        public IEnumerable<Product> GetProductsByRestaurant(int restaurantId)
        {
            return context.Products
                .Include("Restaurant.Owner")
                .Where(p => p.Restaurant.Id == restaurantId)
                .ToList();
        }

        public void RemoveProduct(Product product)
        {
            context.Products.Remove(product);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() >= 0;
        }
    }
}
