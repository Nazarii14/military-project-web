using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  MilitaryProject.Models
{
    public class Brigade
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CommanderName { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public string Location { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<BrigadeStorage> BrigadeStorages { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}