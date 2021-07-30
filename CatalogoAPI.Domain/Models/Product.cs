namespace Catalog.Domain.Models
{
    public class Product
    {
        public Product(int sku, string name, bool freeShipping, string description, decimal price)
        {
            Sku = sku;
            Description = description;
            Name = name;
            FreeShipping = freeShipping;
            Price = price;
        }

        public int Sku { get; }

        public string Name { get; }

        public bool FreeShipping { get; }

        public string Description { get; }

        public decimal Price { get; }

        public int CategoryId { get; set; }
    }
}
