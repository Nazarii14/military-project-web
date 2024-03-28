using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.Weapon
{
    public class WeaponViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }

        [Display(Name = "Type")]

        [Required(ErrorMessage = "Type required")]
        public string Type { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Price required")]
        public decimal Price { get; set; }

        [Display(Name = "Weight")]
        public float Weight { get; set; }
    }
}
