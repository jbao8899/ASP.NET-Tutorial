using System.ComponentModel.DataAnnotations;

namespace API_Consumer.Models.Validations
{
    public class Shirt_EnsureCorrectSizeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            Shirt? shirt = validationContext.ObjectInstance as Shirt;

            if (shirt != null)
            {
                if (shirt.IsForMen && shirt.Size < 8)
                {
                    return new ValidationResult("The size of a shirt for men has to be greater than or equal to 8");
                }
                else if (!shirt.IsForMen && shirt.Size < 6)
                {
                    return new ValidationResult("The size of a shirt for women has to be greater than or equal to 6");
                }
            }

            return ValidationResult.Success;
        }
    }

}
