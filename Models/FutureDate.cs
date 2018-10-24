using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class FutureDateAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime now = DateTime.Now;
            DateTime Date = DateTime.Parse(value.ToString());
            if (Date < now) 
            {
                return new ValidationResult("Invalid date. Please select a date in future!");
            }
            else
                return ValidationResult.Success;
        }
    }
}