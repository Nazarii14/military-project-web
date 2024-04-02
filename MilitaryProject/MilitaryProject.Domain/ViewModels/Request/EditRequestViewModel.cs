using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.Request
{
    public class EditRequestViewModel
    {
        public int ID { get; set; }
        public int WeaponQuantity { get; set; }
        public int AmmunitionQuantity { get; set; }
        public string Message { get; set; }
        public string RequestStatus { get; set; }
    }
}