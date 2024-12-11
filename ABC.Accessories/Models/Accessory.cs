using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.Models {
    public class Accessory {
        public int Id {get; set;}
        [Required]
        public required string Name {get; set;}
        [Required]
        public required string Description {get; set;}
        [Required]
        public Decimal Price {get; set;}
        public string? Specifications {get; set;}
        public DateTime CreatedDate {get; set;}
        public DateTime UpdatedDate {get; set;}
        [Required]
        public required string DeliveryETA {get; set;}

    }
}