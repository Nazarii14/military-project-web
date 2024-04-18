using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.User
{
    public class TwoFAViewModel
    {
        public ClaimsIdentity Claims { get; set; }
        public string QrCode { get; set; }
    }
}
