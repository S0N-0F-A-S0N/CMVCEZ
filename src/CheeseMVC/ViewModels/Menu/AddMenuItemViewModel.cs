using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheeseMVC.ViewModels.Menu
{
    public class AddMenuItemViewModel
    {
        // reference data
        public string MenuName { get; set; } = "";
        public IList<SelectListItem> CheeseChoices { get; set; }

        // form data
        [Required]
        public int MenuID { get; set; }

        [Display(Name = "Cheese ID")]
        [Required]
        public int SelectedCheeseID { get; set; }

        public AddMenuItemViewModel () { }

        public AddMenuItemViewModel(Models.Menu menu, IEnumerable<Cheese> cheeses)
        {
            MenuID = menu.ID;
            MenuName = menu.Name;
            CheeseChoices = new List<SelectListItem>();

            foreach (Cheese cheese in cheeses)
            {
                CheeseChoices.Add(new SelectListItem
                {
                    Text = cheese.Name,
                    Value = cheese.ID.ToString()
                });
            }
        }

        public AddMenuItemViewModel(Cheese selectedCheese, Models.Menu menu, IEnumerable<Cheese> cheeses)
            : this(menu, cheeses)
        {
            SelectedCheeseID = selectedCheese.ID;
        }
    }
}