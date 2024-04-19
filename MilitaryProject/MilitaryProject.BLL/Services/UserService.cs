using Google.Authenticator;
using MilitaryProject.BLL.Interfaces;
using MilitaryProject.DAL.Interface;
using MilitaryProject.DAL.Repositories;
using MilitaryProject.Domain.Entity;
using MilitaryProject.Domain.Enum;
using MilitaryProject.Domain.Helpers;
using MilitaryProject.Domain.Response;
using MilitaryProject.Domain.ViewModels.User;
using Google.Authenticator;
using System.Security.Claims;
using Azure.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using static QRCoder.PayloadGenerator.WiFi;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using AutoMapper;
using System.Security.Cryptography;

namespace MilitaryProject.BLL.Services
{
    public class UserService: IUserService
    {
        private readonly BaseRepository<User> _userRepository;

        public UserService(BaseRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<User>> GetUser(string email)
        {
            var users = await _userRepository.GetAll();
            var user = users.FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                return new BaseResponse<User>()
                {
                    Description = "User not found",
                    StatusCode = StatusCode.NotFound
                };
            }

            return new BaseResponse<User>()
            {
                Data = user,
                StatusCode = StatusCode.OK
            };
        }

        public async Task<BaseResponse<List<User>>> GetAll()
        {
            var users = await _userRepository.GetAll();

            return new BaseResponse<List<User>>()
            {
                Data = users,
                StatusCode = StatusCode.OK
            };
        }

        public async Task<BaseResponse<TwoFAViewModel>> SignUp(SignupViewModel model)
        {
            var userCheck = await CheckUserExistence(model);
            var user = userCheck.Data;

            user = new User()
            {
                Email = model.Email,
                Password = HashPasswordHelper.HashPassword(model.Password),
                Name = model.Name,
                Lastname = model.Lastname,
                Age = model.Age,
                BrigadeID = 106,
                Role = Role.Guest,
            };

            var userEmail = user.Email;
            var key = KeyGeneration(userEmail);
            var qrCodeUrl = await QrCode(model);
            var isVerified = Verify2FA(key, model.TwoFactorSecretKey);

            if (!isVerified)
            {
                return new BaseResponse<TwoFAViewModel>()
                {
                    Description = "Invalid 2FA code",
                };
            }

            await _userRepository.Create(user);
            var result = Authenticate(user);

            return new BaseResponse<TwoFAViewModel>
            {
                Data = new TwoFAViewModel
                {
                    Claims = result,
                    QrCode = qrCodeUrl.Data,
                },
                Description = "User added",
                StatusCode = StatusCode.OK,
            };
        }

        public async Task<BaseResponse<TwoFAViewModel>> Login(LoginViewModel model)
        {
            var response = await CheckCreds(model);
            var user = response.Data;

            var userEmail = user.Email;
            var key = KeyGeneration(userEmail);
            var qrCodeUrl = await QrCode(model);
            //var isVerified = Verify2FA(key, model.TwoFactorSecretKey);
            var isVerified = true;

            if (!isVerified)
            {
                return new BaseResponse<TwoFAViewModel>()
                {
                    Description = "Invalid 2FA code",
                };
            }

            var result = Authenticate(user);

            return new BaseResponse<TwoFAViewModel>
            {
                Data = new TwoFAViewModel
                {
                    Claims = result,
                    QrCode = qrCodeUrl.Data,
                },
                Description = "User logined",
                StatusCode = StatusCode.OK,
            };
        }

        public async Task<BaseResponse<bool>> ChangePassword(RestorePasswordViewModel model)
        {
            var user = await CheckUserExistence(model);
            user.Data.Password = HashPasswordHelper.HashPassword(model.Password);

            await _userRepository.Update(user.Data);

            return new BaseResponse<bool>
            {
                Data = true,
                Description = "User logined",
                StatusCode = StatusCode.OK,
            };
        }

        public async Task<BaseResponse<User>> CheckCreds(LoginViewModel model)
        {
            if (model == null)
            {
                return new BaseResponse<User>
                {
                    Description = "Model is null",
                };
            }

            var users = await _userRepository.GetAll();
            var user = users.FirstOrDefault(x => x.Email == model.Email);

            if (user == null)
            {
                return new BaseResponse<User>
                {
                    Description = "User does not exist",
                };
            }

            if (user.Password != HashPasswordHelper.HashPassword(model.Password)
                || user.Email != model.Email)
            {
                return new BaseResponse<User>()
                {
                    Description = "Invalid password",
                };
            }

            return new BaseResponse<User>
            {
                Data = user,
                StatusCode= StatusCode.OK,
            };
        }

        public async Task<BaseResponse<User>> CheckUserExistence(SignupViewModel model)
        {
            if (model == null)
            {
                return new BaseResponse<User>
                {
                    Description = "Model is null",
                };
            }

            var users = await _userRepository.GetAll();
            var user = users.FirstOrDefault(x => x.Email == model.Email);

            if (user != null)
            {
                return new BaseResponse<User>
                {
                    Description = "User is already exist",
                };
            }

            return new BaseResponse<User>
            {
                Data = user,
                StatusCode= StatusCode.OK,
            };
        }

        public async Task<BaseResponse<User>> CheckUserExistence(RestorePasswordViewModel model)
        {
            if (model == null)
            {
                return new BaseResponse<User>
                {
                    Description = "Model is null",
                };
            }

            var users = await _userRepository.GetAll();
            var user = users.FirstOrDefault(x => x.Email == model.Email);

            if (user == null)
            {
                return new BaseResponse<User>
                {
                    Description = "User does not exist",
                };
            }

            return new BaseResponse<User>
            {
                Data = user,
                StatusCode = StatusCode.OK,
            };
        }

        public async Task<BaseResponse<string>> QrCode(LoginViewModel model)
        {
            var response = await CheckCreds(model);
            var user = response.Data;

            var userEmail = user.Email;
            var qrCodeUrl = GenerateQrCodeUrl(userEmail);

            return new BaseResponse<string>
            {
                Data = qrCodeUrl,
                StatusCode = StatusCode.OK,
            };
        }

        public async Task<BaseResponse<string>> QrCode(SignupViewModel model)
        {
            var qrCodeUrl = GenerateQrCodeUrl(model.Email);

            return new BaseResponse<string>
            {
                Data = qrCodeUrl,
                StatusCode = StatusCode.OK,
            };
        }

        public async Task<BaseResponse<string>> GenerateResetToken(string email)
        {
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            return new BaseResponse<string>()
            {
                Data = token,
                StatusCode = StatusCode.OK
            };
        }

        private string KeyGeneration(string userEmail)
        {
            return userEmail + "secretKey123";
        }

        private string GenerateQrCodeUrl(string userEmail)
        {
            var authenticator = new TwoFactorAuthenticator();
            var key = KeyGeneration(userEmail);
            var setupInfo = authenticator.GenerateSetupCode("MilitaryProject", userEmail, key, false, 100);
            return setupInfo.QrCodeSetupImageUrl;
        }

        private bool Verify2FA(string key, string token)
        {
            var authenticator = new TwoFactorAuthenticator();
            return authenticator.ValidateTwoFactorPIN(key, token);
        }

        private ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
                new Claim("UserID", user.ID.ToString()),
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        public async Task<BaseResponse<bool>> ChangeRole(UserInfoViewModel model)
        {
            var user = await GetUser(model.Email);
            user.Data.Role = model.Role;

            await _userRepository.Update(user.Data);

            return new BaseResponse<bool>
            {
                Data = true,
                Description = "Role changed",
                StatusCode = StatusCode.OK,
            };
        }
    }
}
