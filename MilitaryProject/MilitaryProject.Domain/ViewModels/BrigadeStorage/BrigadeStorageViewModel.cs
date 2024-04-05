using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.BrigadeStorage
{
    public class BrigadeStorageViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Brigade ID")]
        public int BrigadeID { get; set; }

        [Display(Name = "Weapon ID")]
        public int WeaponID { get; set; }

        [Display(Name = "Ammunition ID")]
        public int AmmunitionID { get; set; }

        [Required(ErrorMessage = "The Weapon Quantity field is required.")]
        [Display(Name = "Weapon Quantity:")]
        public int WeaponQuantity { get; set; }

        [Required(ErrorMessage = "The Ammunition Quantity field is required.")]
        [Display(Name = "Ammunition Quantity:")]
        public int AmmunitionQuantity { get; set; }

        [Display(Name = "Weapon Remainder")]
        public int WeaponRemainder { get; set; }

        [Display(Name = "Ammunition Remainder")]
        public int AmmunitionRemainder { get; set; }
    }
}