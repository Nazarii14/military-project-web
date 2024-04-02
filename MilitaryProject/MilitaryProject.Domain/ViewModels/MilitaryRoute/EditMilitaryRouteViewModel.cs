using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.MilitaryRoute
{
    public class EditMilitaryRouteViewModel
    {
        public int ID { get; set; }
        public string StartPoint { get; set; }
        public string Destination { get; set; }
        public int WeaponQuantity { get; set; }
        public int AmmunitionQuantity { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
