using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.Dtos
{
    public class PutProductInput
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name must not be empty.")]
        public string Name { get; set; }

        [Required]
        public bool FreeShipping { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Description must not be empty.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Zero is not a valid Category Id.")]
        [Required(ErrorMessage = "Category Id is required.")]
        public int CategoryId { get; set; }
    }
}
