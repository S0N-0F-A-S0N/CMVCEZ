﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels.Menu
{
    public class AddMenuViewModel
    {
        [Display(Name = "Menu Name")]
        [Required]
        public string Name { get; set; }
    }
}
