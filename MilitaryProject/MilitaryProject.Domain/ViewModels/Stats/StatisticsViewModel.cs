using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace MilitaryProject.Domain.ViewModels.Stats
{
    public class StatisticsViewModel
    {
        public Dictionary<string, int> WeaponTypeCounts { get; set; } = new Dictionary<string, int>();

        public Dictionary<float, int> WeaponWeightCounts { get; set; } = new Dictionary<float, int>();

        public Dictionary<decimal, int> WeaponPriceCount { get; set; } = new Dictionary<decimal, int>();

        public Dictionary<string, int> AmmunitionTypeCounts { get; set; } = new Dictionary<string, int>();

        public Dictionary<string, int> AmmunitionSizeCounts { get; set; } = new Dictionary<string, int>();

        public Dictionary<decimal, int> AmmunitionPriceCount { get; set; } = new Dictionary<decimal, int>();
    }
}
