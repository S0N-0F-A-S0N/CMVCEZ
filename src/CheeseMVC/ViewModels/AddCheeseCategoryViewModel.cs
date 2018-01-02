using System.ComponentModel.DataAnnotations;

namespace CheeseMVC.ViewModels
{
    public class AddCheeseCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}