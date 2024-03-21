using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.Brigade
{
    public class ReadBrigadeViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CommanderName { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public string Location { get; set; }
    }
}
