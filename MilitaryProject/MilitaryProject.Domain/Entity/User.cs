using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MilitaryProject.Domain.Enum;

namespace MilitaryProject.Domain.Entity
{
  public class User
    {
        public int ID { get; set; }
        public int BrigadeID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public int Age { get; set; }

        public virtual Brigade Brigade { get; set; }
        public virtual ICollection<UserItems> UserItems { get; set; }
    }
}