using RestaurantService.Models;
using System.Collections.Generic;

namespace RestaurantService.Data
{
    public interface IProductRepo
    {
        bool SaveChanges();
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByRestaurant(int restaurantId);
        IEnumerable<Product> GetProductsByIdList(IEnumerable<int> restaurantIds);
        Product GetProductById(int id);
        void CreateProduct(Product product);
        void RemoveProduct(Product product);
    }
}
