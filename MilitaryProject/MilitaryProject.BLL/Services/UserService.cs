using MilitaryProject.BLL.Interfaces;
using MilitaryProject.DAL.Interface;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Helpers;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;

namespace MilitaryProject.BLL.Services
{
    public class UserService: IUserService
    {
        private readonly BaseRepository<User> _userRepository;

        public UserService(BaseRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        private ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        public async Task<BaseResponse<ClaimsIdentity>> SignUp(SignupViewModel model)
        {
            var users = await _userRepository.GetAll();
            var user = users.FirstOrDefault(x => x.Email == model.Email);

            if (model == null)
            {
                return new BaseResponse<ClaimsIdentity>
                {
                    Description = "Model is null",
                };
            }

            if (user != null)
            {
                return new BaseResponse<ClaimsIdentity>
                {
                    Description = "User is already exist",
                };
            }

            user = new User()
            {
                Email = model.Email,
                Password = HashPasswordHelper.HashPassword(model.Password),
                Name = model.Name,
                Lastname = model.Lastname,
                Age = model.Age,
                //BrigadeID = 1,
                Role = Role.Guest,
            };

            await _userRepository.Create(user);
            var result = Authenticate(user);

            return new BaseResponse<ClaimsIdentity>
            {
                Data = result,
                Description = "User added",
                StatusCode = StatusCode.OK,
            };
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Description = "Model is null",
                    };
                }

                var users = await _userRepository.GetAll();
                var user = users.FirstOrDefault(x => x.Email == model.Email);

                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Description = "User does not exist",
                    };
                }

                if (user.Password != HashPasswordHelper.HashPassword(model.Password)
                    || user.Email != model.Email)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Invalid password",
                    };
                }

                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>
                {
                    Data = result,
                    Description = "User logined",
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = $"[Login] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
