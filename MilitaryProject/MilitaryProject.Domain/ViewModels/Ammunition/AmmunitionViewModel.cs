using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.Ammunition
{
    public class AmmunitionViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Type required")]
        [Display(Name = "Type")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price required")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Size required")]
        [Display(Name = "Size")]
        public string Size { get; set; }
    }
}
