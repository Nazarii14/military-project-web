using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.Stats
{
    public class StatsAmmunitionViewModel
    {
        public int AmmunitionID { get; set; }
        public int UserID { get; set; }
        public string AmmunitionName { get; set; }
        public string AmmunitionType { get; set; }
        public string AmmunitionSize { get; set; }
        public decimal AmmunitionPrice { get; set; }
        public int AmmunitionCount { get; set; }
        public int NeededAmmunitionCount { get; set; }
    }
}
