namespace Catalog.Domain.Models
{
    public class Product
    {
        public Product(int lm, string name, bool freeShipping, string description, decimal price)
        {
            Lm = lm;
            Description = description;
            Name = name;
            FreeShipping = freeShipping;
            Price = price;
        }

        public int Lm { get; }

        public string Name { get; }

        public bool FreeShipping { get; }

        public string Description { get; }

        public decimal Price { get; }

        public int CategoryId { get; set; }
    }
}
