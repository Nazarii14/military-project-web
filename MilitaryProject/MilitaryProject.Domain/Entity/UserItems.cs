using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MilitaryProject.Domain.Entity
{
   public class UserItems
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int WeaponID { get; set; }
        public int AmmunitionID { get; set; }

        public virtual User User { get; set; }
        public virtual Weapon Weapon { get; set; }
        public virtual Ammunition Ammunition { get; set; }
    }
}