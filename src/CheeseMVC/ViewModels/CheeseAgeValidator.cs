using System.ComponentModel.DataAnnotations;

namespace CheeseMVC.ViewModels
{
    /// <summary>
    /// Simple custom validator class with static validation method.
    /// Use with CustomValidator() attribute
    /// </summary>
    public class CheeseAgeValidator
    {
        public static ValidationResult IsValidAge(int age)
        {
            if (age < 3)
            {
                return new ValidationResult("This cheese hasn't aged enough!");
            }

            if (age > 25)
            {
                return new ValidationResult("Look, it may have mold but... that's a little old!");
            }

            return ValidationResult.Success;
        }
    }
}