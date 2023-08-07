using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Models.Validations
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
