using MilitaryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.User
{
    public class UserInfoViewModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [ForeignKey("Brigade")]
        public int BrigadeID { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
