using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.UserItems
{
    public class UserItemsViewModel
    {
        public int ID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        [ForeignKey("Weapon")]
        public int WeaponID { get; set; }
        [ForeignKey("Ammunition")]
        public int AmmunitionID { get; set; }
    }
}