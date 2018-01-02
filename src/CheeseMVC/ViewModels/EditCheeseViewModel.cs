using System.Collections.Generic;
using CheeseMVC.Models;

namespace CheeseMVC.ViewModels
{
    public class EditCheeseViewModel : AddCheeseViewModel
    {
        public int CheeseId { get; set; }

        public EditCheeseViewModel()
        {
        }

        public EditCheeseViewModel(Cheese cheese, List<CheeseCategory> categories) : base(categories)
        {
            CheeseId = cheese.ID;
            Name = cheese.Name;
            Description = cheese.Description;
            SelectedCheeseCategoryID = cheese.CategoryID;
            Rating = cheese.Rating;
            Odor = cheese.Odor;
            Age = cheese.Age;
        }
    }
}