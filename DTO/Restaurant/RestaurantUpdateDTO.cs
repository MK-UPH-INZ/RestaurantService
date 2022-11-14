using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.DTO.Restaurant
{
    public class RestaurantUpdateDTO
    {
        public string Name { get; set; }

        public int? OwnerId { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        [RegularExpression(@"^[0-9]{2}-[0-9]{3}$", ErrorMessage = "Postal code format ##-###")]
        public string PostalCode { get; set; }

        public string Voivodeship { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegisterDate { get; set; }
    }
}
