using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.BLL.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<ClaimsIdentity>> SignUp(SignupViewModel model);
        Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);
    }
}
