using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Domain.Models
{
    public class Product
    {
        public Product(int lm, string name, bool freeShipping, string description, decimal price, int categoryId)
        {
            Lm = lm;
            Description = description;
            Name = name;
            FreeShipping = freeShipping;
            Price = price;
            CategoryId = categoryId;
        }

        [BsonId]
        public int Lm { get; }

        [BsonElement("Name")]
        public string Name { get; }

        [BsonElement("FreeShipping")]
        public bool FreeShipping { get; }

        [BsonElement("Description")]
        public string Description { get; }

        [BsonElement("Price")]
        public decimal Price { get; }

        [BsonElement("CategoryId")]
        public int CategoryId { get; }

        public override bool Equals(object obj)
        {
            if (!(obj is Product prod))
                return false;

            return prod.Lm == Lm;
        }

        public override int GetHashCode()
        {
            return Lm.GetHashCode();
        }
    }
}
