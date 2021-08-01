namespace Catalog.Application.Dtos
{
    public class PutProductInput
    {
        public string Name { get; }

        public bool FreeShipping { get; }

        public string Description { get; }

        public decimal Price { get; }

        public int CategoryId { get; set; }
    }
}
