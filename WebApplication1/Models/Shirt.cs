using WebApplication1.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Shirt
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? BrandName { get; set; } = null;

        [Required]
        public string? Color { get; set; } = null;

        [Shirt_EnsureCorrectSize]
        public int? Size { get; set; } = -1;

        [Required]
        public bool IsForMen { get; set; } = true;

        public double? Price { get; set; } = -1.0;

        public override string ToString()
        {
            string retString = $"This shirt has an ID of {Id}";


            if (BrandName != null) {
                retString += $", was made by {BrandName}";
            }
            else
            {
                retString += ", was made by an unknown brand";
            }

            if (Color != null)
            {
                retString += $", is {Color}";
            }
            else {
                retString += ", has an unknown color";
            }

            if (Size >= 0)
            {
                retString += $", has a size of {Size}";
            }
            else
            {
                retString += ", has an unknown size";
            }

            if (IsForMen)
            {
                retString += ", was designed for men";
            }
            else
            {
                retString += ", was designed for women";
            }

            if (Price >= 0.0)
            {
                retString += $", and costs {Price}.";
            }
            else
            {
                retString += ", and costs an unknown amount.";
            }

            return retString;
        }
    }
}
