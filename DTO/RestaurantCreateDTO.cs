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
    }
}
