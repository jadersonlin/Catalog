namespace Catalog.Application.Dtos
{
    public class PutProductInput
    {
        public string Name { get; set; }

        public bool FreeShipping { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }
    }
}
