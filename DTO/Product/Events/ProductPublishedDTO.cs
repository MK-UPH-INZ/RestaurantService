namespace RestaurantService.DTO.Product.Events
{
    public class ProductPublishedDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Event { get; set; } = "Product_Published";
    }
}
