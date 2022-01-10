using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Utilites.Validators
{
    public class LessThan : ValidationAttribute
    {
        private readonly string endDate;

        public LessThan(string endDate)
        {
            this.endDate = endDate;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime Start = (DateTime)value;

            var reflectionProperty = validationContext.ObjectType.GetProperty(endDate);

            if (reflectionProperty == null)
            {
                throw new ArgumentException("Property {" + endDate + "} could not be found");
            }

            DateTime End = (DateTime)reflectionProperty.GetValue(validationContext.ObjectInstance);

            if (Start > End)
            {
                return new ValidationResult("Start date must be less than or equal to the end date.");
            }

            return ValidationResult.Success;
        }
    }
}
