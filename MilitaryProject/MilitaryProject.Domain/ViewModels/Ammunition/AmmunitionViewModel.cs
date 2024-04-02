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
      public string Type { get; set; }
      public string Name { get; set; }
      public decimal Price { get; set; }
      public string Size { get; set; }
    }
}
