using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CheeseMVC.Data;
using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheeseMVC.ViewModels
{
    public class AddCheeseViewModel
    {
        //
        // model binding properties
        //

        // ProhibitedCheeseValidatorAttribute.cs
        [ProhibitedCheeseValidator(new[] { "provel" })]
        [Required]
        [Display(Name = "Cheese Name")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Describe your cheese, sir!")]
        public string Description { get; set; } = "";

        [Display(Name = "Cheese Category")]
        public int SelectedCheeseCategoryID { get; set; }

        public List<SelectListItem> CheeseCategories { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } = 5;

        // OdorValidatorAttribute.cs
        [OdorValidator]
        [Required]
        public string Odor { get; set; }

        // CheeseAgeValidator.cs
        [CustomValidation(typeof(CheeseAgeValidator), "IsValidAge")]
        public int Age { get; set; }

        public Cheese CreateCheese(IList<CheeseCategory> categories)
        {
            return new Cheese()
            {
                Name = Name,
                Description = Description,
                Rating = Rating,
                Odor = Odor,
                Age = Age,
                Category = categories.Single(c => c.ID == SelectedCheeseCategoryID)
            };

        }

        public AddCheeseViewModel()
        {
            CheeseCategories = new List<SelectListItem>();
        }

        public AddCheeseViewModel(List<CheeseCategory> categories) : this()
        {
            foreach (CheeseCategory category in categories)
            {
                // <option value="ID">Name</option>
                CheeseCategories.Add(new SelectListItem()
                {
                    Value = category.ID.ToString(),
                    Text = category.Name
                });
            }
        }
    }
}