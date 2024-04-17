﻿using Google.Authenticator;
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

namespace MilitaryProject.BLL.Services
{
    public class UserService: IUserService
    {
        private readonly BaseRepository<User> _userRepository;

        public UserService(BaseRepository<User> userRepository)
        {
            _userRepository = userRepository;
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
                BrigadeID = 1,
                Role = Role.Guest,
            };

            var userEmail = user.Email;
            var key = KeyGeneration(userEmail);
            var qrCodeUrl = GenerateQrCodeUrl(userEmail);
            var isVerified = Verify2FA(key, model.TwoFactorSecretKey);

            if (!isVerified)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Invalid 2FA code",
                };
            }

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

            var userEmail = user.Email;
            var key = KeyGeneration(userEmail);
            var qrCodeUrl = GenerateQrCodeUrl(userEmail);
            var isVerified = Verify2FA(key, model.TwoFactorSecretKey);

            if (!isVerified)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Invalid 2FA code",
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

        private string KeyGeneration(string userEmail)
        {
            return userEmail + "secretKey123";
        }

        private string GenerateQrCodeUrl(string userEmail)
        {
            var authenticator = new TwoFactorAuthenticator();
            var key = KeyGeneration(userEmail);
            var setupInfo = authenticator.GenerateSetupCode("MilitaryProject", userEmail, key, false, 300);
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
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
