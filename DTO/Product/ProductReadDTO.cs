namespace RestaurantService.DTO.Product
{
    public class ProductReadDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public virtual ProductRestaurantDTO Restaurant { get; set; }
    }
}
