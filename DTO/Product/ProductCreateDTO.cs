using System.ComponentModel.DataAnnotations;

namespace RestaurantService.DTO.Product
{
    public class ProductCreateDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        [Required]
        public int RestaurantId { get; set; }
    }
}
