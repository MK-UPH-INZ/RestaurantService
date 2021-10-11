using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.DTO
{
    public class RestaurantCreateDTO
    {
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
    }
}
