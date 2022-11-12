using RestaurantService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.DTO
{
    public class RestaurantReadDTO
    {
        public int Id { get; set; }

        public RestaurantUserDTO Owner { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Voivodeship { get; set; }

        public DateTime RegisterDate { get; set; }
    }
}
