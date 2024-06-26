﻿using MilitaryProject.Domain.Entity;
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
        Task<BaseResponse<User>> GetUser(string email);
        Task<BaseResponse<User>> GetUser(int id);
        Task<BaseResponse<List<User>>> GetAll();
        Task<BaseResponse<bool>> DeleteUser(int id);
        Task<BaseResponse<TwoFAViewModel>> SignUp(SignupViewModel model);
        Task<BaseResponse<TwoFAViewModel>> Login(LoginViewModel model);
        Task<BaseResponse<User>> CheckCreds(LoginViewModel model);
        Task<BaseResponse<string>> QrCode(LoginViewModel model);
        Task<BaseResponse<string>> QrCode(SignupViewModel model);
        Task<BaseResponse<User>> CheckUserExistence(SignupViewModel model);
        Task<BaseResponse<User>> CheckUserExistence(RestorePasswordViewModel model);
        Task<BaseResponse<string>> GenerateResetToken(string email);
        Task<BaseResponse<bool>> ChangePassword(RestorePasswordViewModel model);
    }
}
