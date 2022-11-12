using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantService.Models 
{
    public class Restaurant
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{2}-[0-9]{3}$", ErrorMessage = "Postal code format ##-###")]
        public string PostalCode { get; set; }

        public string Voivodeship { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime RegisterDate { get; set; }

        public virtual User Owner { get; set; }
    }
}