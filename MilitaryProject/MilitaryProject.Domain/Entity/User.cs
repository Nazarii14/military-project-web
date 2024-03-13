using MilitaryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.Entity
{
    public class User
    {
        public long Id { get; set; }
        public long? BrigadeID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }      
        public Role Role { get; set; }
    }
}
