using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  MilitaryProject.Models
{
  public class BrigadeStorage
    {
        public int ID { get; set; }
        public int BrigadeID { get; set; }
        public int WeaponID { get; set; }
        public int AmmunitionID { get; set; }
        public int WeaponQuantity { get; set; }
        public int AmmunitionQuantity { get; set; }
        public int WeaponRemainder { get; set; }
        public int AmmunitionRemainder { get; set; }

        public virtual Brigade Brigade { get; set; }
        public virtual Weapon Weapon { get; set; }
        public virtual Ammunition Ammunition { get; set; }
    }
}