namespace RestaurantService.DTO.Product.Events
{
    public class ProductUpdatedDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Event { get; set; } = "Product_Updated";
    }
}
