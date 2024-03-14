using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  MilitaryProject.Models
{
  public class Weapon
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public float Weight { get; set; }

        public virtual ICollection<UserItems> UserItems { get; set; }
        public virtual ICollection<BrigadeStorage> BrigadeStorages { get; set; }
        public virtual ICollection<MilitaryRoute> MilitaryRoutes { get; set; }
    }
}