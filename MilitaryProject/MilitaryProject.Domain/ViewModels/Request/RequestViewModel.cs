using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.Request
{
    public class RequestViewModel
    {
        public int ID { get; set; }
        public int BrigadeID { get; set; }
        public int WeaponID { get; set; }
        public int AmmunitionID { get; set; }
        public int WeaponQuantity { get; set; }
        public int AmmunitionQuantity { get; set; }
        public string Message { get; set; }
        public string RequestStatus { get; set; }
    }
}
