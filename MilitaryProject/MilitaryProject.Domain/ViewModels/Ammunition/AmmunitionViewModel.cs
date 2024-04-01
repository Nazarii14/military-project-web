using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MilitaryProject.Domain.ViewModels.Ammunition
{
    public class AmmunitionViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "Type required")]
        public string Type { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Price required")]
        public decimal Price { get; set; }

        [Display(Name = "Size")]
        [Required(ErrorMessage = "Size required")]
        public string Size { get; set; }
    }
}
