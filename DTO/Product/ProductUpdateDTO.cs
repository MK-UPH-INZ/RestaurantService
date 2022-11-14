using System.ComponentModel.DataAnnotations;

namespace RestaurantService.DTO.Product
{
    public class ProductUpdateDTO
    {
        public string Name { get; set; }

        public int? Price { get; set; }
    }
}
