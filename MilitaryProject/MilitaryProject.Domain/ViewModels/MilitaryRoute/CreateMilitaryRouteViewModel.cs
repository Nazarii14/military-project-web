using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.MilitaryRoute
{
    public class CreateMilitaryRouteViewModel
    {
        public int ID { get; set; }
        [ForeignKey("User")]
        public int VolunteerID { get; set; }
        [ForeignKey("Weapon")]
        public int WeaponID { get; set; }
        [ForeignKey("Ammunition")]
        public int AmmunitionID { get; set; }
        public string StartPoint { get; set; }
        public string Destination { get; set; }
        public int WeaponQuantity { get; set; }
        public int AmmunitionQuantity { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string VolunteerLastName { get; set; }
        public string WeaponName { get; set; }
        public string AmmunitionName { get; set; }
    }
}
