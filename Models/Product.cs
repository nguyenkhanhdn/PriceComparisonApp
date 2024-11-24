namespace PriceComparisonApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Branch { get; set; }
        public required string Shop { get; set; }
        public required string Category { get; set; }
        public required string ImgUrl { get; set; }
        public int Memory { get; set; }
        public required float Price{ get; set; }
        public float Rate { get; set; }
    }
}
