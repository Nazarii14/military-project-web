using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.Request
{
    public class RequestViewModel
    {
        public int ID { get; set; }
        [ForeignKey("Brigade")]
        public int BrigadeID { get; set; }
        [ForeignKey("Weapon")]
        public int WeaponID { get; set; }
        [ForeignKey("Ammunition")]
        public int AmmunitionID { get; set; }
        public int WeaponQuantity { get; set; }
        public int AmmunitionQuantity { get; set; }
        public string Message { get; set; }
        public string RequestStatus { get; set; }
    }
}
