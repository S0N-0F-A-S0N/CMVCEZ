using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using CheeseMVC.ViewModels.Menu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private CheeseDbContext context;

        // MVC framework will call this constructor with the 
        // appropriate context instance, which was registered as
        // a framework service in Startup.cs
        public MenuController(CheeseDbContext context)
        {
            this.context = context;
        }

        [Route("/menu", Name = "AllMenus")]
        public IActionResult Index()
        {
            List<Menu> menus = context.Menus.ToList();
            return View("Menus", menus);
        }

        [Route("/menu/{menuID}")]
        public IActionResult Index(int menuID)
        {
            List<Cheese> cheeses = context.CheeseMenus
                .Include(m => m.Cheese)
                .Where(m => m.MenuID == menuID)
                .Select(m => m.Cheese)
                .ToList();

            Menu menu = context.Menus
                .Single(m => m.ID == menuID);

            return View("Menu", new ViewMenuViewModel(menu, cheeses));
        }

        [Route("/menu/add")]
        public IActionResult Add()
        {
            return View(new AddMenuViewModel());
        }

        [HttpPost]
        [Route("/menu/add")]
        public IActionResult Add(AddMenuViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", viewModel);
            }

            Menu menu = new Menu();
            menu.Name = viewModel.Name;
            context.Menus.Add(menu);
            context.SaveChanges();

            return Redirect("/menu");
        }

        [Route("/menu/{menuID}/item/add")]
        public IActionResult AddItem(int menuID)
        {
            Menu menu = context.Menus.Single(m => m.ID == menuID);
            List<Cheese> availableCheeses = context.Cheeses.ToList();
            return View(new AddMenuItemViewModel(menu, availableCheeses));
        }

        [HttpPost]
        [Route("/menu/{menuID}/item/add")]
        public IActionResult AddItem(AddMenuItemViewModel viewModel)
        {
            Menu menu = context.Menus
                .Include(m => m.CheeseMenus)
                .Single(m => m.ID == viewModel.MenuID);
            List<Cheese> availableCheeses = context.Cheeses.ToList();
            Cheese selectedCheese = availableCheeses.Single(c => c.ID == viewModel.SelectedCheeseID);

            if (!ModelState.IsValid)
            {
                viewModel = new AddMenuItemViewModel(
                    selectedCheese,
                    menu,
                    availableCheeses
                );
                return View("AddItem", viewModel);
            }

            // check to see if we've already assigned the selected
            // cheese to the menu--if so, just redirect
            int existingMenuCount = menu.CheeseMenus
                .Count(cm => cm.CheeseID == selectedCheese.ID);

            if (existingMenuCount > 0)
            {
                return Redirect(
                    String.Format("/menu/{0}", viewModel.MenuID)
                );
            }

            // add cheese to menu
            CheeseMenu cheeseMenu = new CheeseMenu
            {
                Menu = menu,
                Cheese = selectedCheese
            };

            // will throw an exception if there is already a CheeseMenu
            // record associated with the given Menu and Cheese
            context.CheeseMenus.Add(cheeseMenu);
            context.SaveChanges();

            return Redirect(
                String.Format("/menu/{0}", viewModel.MenuID)
            );
        }
    }
}
