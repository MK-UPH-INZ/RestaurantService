using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantService.Models
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ExternalId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        public virtual List<Restaurant> UserRestaurants { get; set; }
    }
}
