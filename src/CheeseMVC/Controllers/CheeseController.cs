using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    // view location: /Views/Cheese/*
    public class CheeseController : Controller
    {
        private CheeseDbContext context;

        // MVC framework will call this constructor with the 
        // appropriate context instance, which was registered as
        // a framework service in Startup.cs
        public CheeseController(CheeseDbContext context)
        {
            this.context = context;
        }

        // Display the list of cheeses
        // GET: /cheese
        public IActionResult Index()
        {
            // data for the view
            IList<Cheese> cheeses = context.Cheeses.Include(c => c.Category).ToList();

            // view defaults to action name: Index.cshtml
            // /Views/Cheese/Index.cshtml
            return View(cheeses);
        }

        public IActionResult Add()
        {
            AddCheeseViewModel viewModel = new AddCheeseViewModel(context.Categories.ToList());

            // /Views/Cheese/Add.cshtml
            return View(viewModel);
        }

        // by default, the route will be: /Cheese/NewCheese
        // but we want to post the form to the same route
        // from which we loaded it: /Cheese/Add
        //
        // Since the Cheese class has name and description
        // attributes, we can let MVC create a new Cheese
        // object for us, and fill its Name and Descirption
        // properties automagically. Note that:
        // - the Cheese class must have a *default* parameterless
        //   constructor, and 
        // - this does *not* yet perform any validation!
        //
        [Route("/cheese/add")]
        [HttpPost]
        public IActionResult NewCheese(AddCheeseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", viewModel);
            }

            // add new cheese to static class list
            Cheese newCheese = viewModel.CreateCheese(context.Categories.ToList());
            context.Cheeses.Add(newCheese);
            context.SaveChanges();

            // go back to the list of cheeses
            return Redirect("/cheese");
        }

        [Route("/cheese/remove")]
        [HttpGet]
        public IActionResult Remove()
        {
            ViewBag.title = "Remove Cheeses";
            ViewBag.cheeses = context.Cheeses.ToList();
            return View("Remove");
        }

        [Route("/cheese/remove")]
        [HttpPost]
        public IActionResult RemoveManyCheeses(int[] cheeseIds)
        {
            foreach (int submittedCheeseId in cheeseIds)
            {
                Cheese cheeseToRemove = context.Cheeses.Single(c => c.ID == submittedCheeseId);
                context.Cheeses.Remove(cheeseToRemove);
            }
            context.SaveChanges();

            return Redirect("/cheese");
        }

        [Route("/cheese/remove/{cheeseId}")]
        [HttpGet]
        public IActionResult RemoveSingleCheese(int cheeseId)
        {
            Cheese cheeseToRemove = context.Cheeses.Single(c => c.ID == cheeseId);
            context.Cheeses.Remove(cheeseToRemove);
            context.SaveChanges();

            return Redirect("/cheese");
        }

        [Route("/cheese/edit")]
        [HttpGet]
        public IActionResult Edit(int cheeseId)
        {
            Cheese cheese = context.Cheeses.Single(c => c.ID == cheeseId);
            EditCheeseViewModel viewModel = new EditCheeseViewModel(cheese, context.Categories.ToList());
            return View(viewModel);
        }

        [Route("/cheese/edit")]
        [HttpPost]
        public IActionResult Edit(EditCheeseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", viewModel);
            }

            Cheese cheese = context.Cheeses.Single(c => c.ID == viewModel.CheeseId);
            cheese.Name = viewModel.Name;
            cheese.Description = viewModel.Description;
            cheese.Category = context.Categories.Single(c => c.ID == viewModel.SelectedCheeseCategoryID);
            cheese.Rating = viewModel.Rating;
            cheese.Odor = viewModel.Odor;
            cheese.Age = viewModel.Age;
            context.SaveChanges();
            return Redirect("/cheese");
        }

        [Route("/cheese/category")]
        public IActionResult Categories()
        {
            return View("CategoryList", context.Categories.ToList());
        }

        [HttpPost]
        [Route("/cheese/category")]
        public IActionResult Category(AddCheeseCategoryViewModel viewModel)
        {
            CheeseCategory category = new CheeseCategory
            {
                Name = viewModel.Name
            };

            context.Categories.Add(category);
            context.SaveChanges();

            return Json(new {id = category.ID});
        }

        [Route("/cheese/category/{categoryID}")]
        public IActionResult Category(int categoryID)
        {
            if (categoryID == 0)
            {
                return Redirect("/category");
            }

            CheeseCategory theCategory = context.Categories.Include(c => c.Cheeses).Single(c => c.ID == categoryID);

            return View(theCategory);
        }
    }
}
