using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilitaryProject.Domain.Entity;

namespace MilitaryProject.Domain.ViewModels.Admin
{
    public class AdminPannelViewModel
    {
        public List<Domain.Entity.User> Users { get; set; }
        public List<Domain.Entity.Brigade> Brigades { get; set; }
        public List<Domain.Entity.Request> Requests { get; set; }
    }
}
