﻿namespace RestaurantService.DTO
{
    public class RestaurantUpdatedDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Event { get; set; } = "Restaurant_Updated";
    }
}