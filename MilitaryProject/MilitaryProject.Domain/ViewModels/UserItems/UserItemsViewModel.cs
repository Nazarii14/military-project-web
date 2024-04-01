using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.UserItems
{
    public class UserItemsViewModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int WeaponID { get; set; }
        public int AmmunitionID { get; set; }
    }
}