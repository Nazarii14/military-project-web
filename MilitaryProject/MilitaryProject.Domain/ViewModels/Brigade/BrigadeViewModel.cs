using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.Brigade
{
    public class BrigadeViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        [Display(Name = "Commander Name")]
        public string CommanderName { get; set; }

        [Display(Name = "Establishment Date")]
        [DataType(DataType.Date)]
        public DateTime EstablishmentDate { get; set; }

        public string Location { get; set; }
    }
}
