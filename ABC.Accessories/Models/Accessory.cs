using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.Models;

    public class Accessory
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public Decimal SellerPrice { get; set; }

        [Required]
        public Decimal AbcPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int SubCategoryId { get; set; }
        [Required]
        public int BrandId { get; set; }


    }
