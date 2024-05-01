using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.Stats
{
    public class StatsWeaponViewModel
    {
        public int WeaponID { get; set; }
        public int UserID { get; set; }
        public string WeaponName { get; set; }
        public string WeaponType { get; set; }
        public float WeaponWeight { get; set; }
        public decimal WeaponPrice { get; set; }
        public int WeaponCount { get; set; }
        public int NeededWeaponCount { get; set; }
    }
}
