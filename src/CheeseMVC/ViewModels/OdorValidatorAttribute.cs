using System;
using System.ComponentModel.DataAnnotations;

namespace CheeseMVC.ViewModels
{
    /// <summary>
    /// Complex custom validation example
    /// </summary>
    public class OdorValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // this validation attribute should only be used on
            // AddCheeseViewModel and its subclasses, because we
            // will check other properties on the view model
            if (!(validationContext.ObjectInstance is AddCheeseViewModel))
            {
                return new ValidationResult("This only works on AddCheeseViewModel and its subclasses!");
            }

            string odor = ((string) value).ToLower();

            // the user must supply an odor!
            if (String.IsNullOrEmpty(odor))
            {
                return new ValidationResult("This cheese needs an odor!");
            }

            AddCheeseViewModel viewModel = validationContext.ObjectInstance as AddCheeseViewModel;
            string cheeseName = viewModel.Name.ToLower();

            // if the cheese name is limburger and the odor is
            // not stinky, validation fails!
            if (cheeseName.Equals("limburger") && !odor.Equals("stinky"))
            {
                return new ValidationResult("You sit on a throne of lies! Limburger is stinky!");
            }

            // otherwise, validation succeeds
            return ValidationResult.Success;
        }
    }
}