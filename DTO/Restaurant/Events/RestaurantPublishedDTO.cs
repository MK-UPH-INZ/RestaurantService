using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.DTO.Restaurant.Events
{
    public class RestaurantPublishedDTO
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        public string Name { get; set; }

        public string Event { get; set; } = "Restaurant_Published";
    }
}
