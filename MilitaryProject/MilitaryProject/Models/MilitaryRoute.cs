using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  MilitaryProject.Models
{
  public class MilitaryRoute
    {
        public int ID { get; set; }
        public int VolunteerID { get; set; }
        public int WeaponID { get; set; }
        public int AmmunitionID { get; set; }
        public string StartPoint { get; set; }
        public string Destination { get; set; }
        public int WeaponQuantity { get; set; }
        public int AmmunitionQuantity { get; set; }
        public DateTime DeliveryDate { get; set; }

        public virtual User Volunteer { get; set; }
        public virtual Weapon Weapon { get; set; }
        public virtual Ammunition Ammunition { get; set; }
    }
}