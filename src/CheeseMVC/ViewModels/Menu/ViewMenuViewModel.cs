using System;
using System.Collections.Generic;
using System.Linq;
using CheeseMVC.Models;

namespace CheeseMVC.ViewModels.Menu
{
    public class ViewMenuViewModel
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public IList<String> CheeseNames { get; set; }

        public ViewMenuViewModel(Models.Menu menu, List<Cheese> cheeses)
        {
            MenuID = menu.ID;
            MenuName = menu.Name;
            CheeseNames = cheeses.Select(c => c.Name).ToList();
        }
    }
}